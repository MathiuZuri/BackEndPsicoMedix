using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.ATENCIONES;
using psicomedixMonolito.Services.Background;
using psicomedixMonolito.Services.Documents.Comprobantes.Pdfservicios;
using psicomedixMonolito.Services.Documents.Comprobantes.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Services.imp.ATENCIONES;
using psicomedixMonolito.Services.imp.WhastAppImp;
using psicomedixMonolito.Services.imp.WhatsApp;
using psicomedixMonolito.Services.Interfacespdf;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Configurations;
using psicomedixMonolito.Utils.Filters;
using psicomedixMonolito.Utils.Helpers;
using psicomedixMonolito.Utils.Hubs;
using psicomedixMonolito.Utils.Middlewares;
using QuestPDF.Infrastructure;
// Namespaces unificados del Monolito
using IPermisoService = psicomedixMonolito.Services.IPermisoService;
using IRolService = psicomedixMonolito.Services.IRolService;

var builder = WebApplication.CreateBuilder(args);

// ==========================================================
// CONFIGURACIÓN DE FILTROS Y CONTROLADORES
// ==========================================================
builder.Services.AddScoped<AuditoriaAutomaticaFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<AuditoriaAutomaticaFilter>();
});

// Respuestas de validación del modelo personalizadas
builder.Services.Configure<ApiBehaviorOptions>(options => {
    // Vincula tu clase de configuración estática del monolito
    // ValidationResponseConfig.ConfigurarRespuestasDeValidacion
});

builder.Services.AddEndpointsApiExplorer();

// ==========================================================
// CONFIGURACIÓN DE SWAGGER (DOCUMENTACIÓN XML)
// ==========================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Psicomedix API",
        Version = "v1",
        Description = "API del Sistema Monolítico Integral de Gestión Clínica - Psicomedix."
    });

    // Inclusión nativa de comentarios XML de los controladores para la documentación de la API
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<JwtHelper>();

// ==========================================================
// CONFIGURACIÓN DE AUTENTICACIÓN JWT
// ==========================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

// ==========================================================
// AUTORIZACIÓN BASADA EN REQUISITOS DE PERMISOS (CLAIMS)
// ==========================================================
builder.Services.AddAuthorization(options =>
{
    foreach (var permiso in PermisosPolicies.Todos)
    {
        options.AddPolicy(permiso, policy =>
            policy.RequireClaim("permiso", permiso));
    }
});

// ==========================================================
// INFRAESTRUCTURA DE DATOS (EF CORE CORRESPONDIENTE A POSTGRESQL)
// ==========================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================================
// REGISTRO DE SERVICIOS DE APLICACIÓN (LÓGICA DE NEGOCIO DIRECTA)
// ==========================================================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IPermisoService, PermisoService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();

builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IHorarioDoctorService, HorarioDoctorService>();
builder.Services.AddScoped<ICitaService, CitaService>();

builder.Services.AddScoped<IServicioClinicoService, ServicioClinicoService>();
builder.Services.AddScoped<IHistorialClinicoService, HistorialClinicoService>();
builder.Services.AddScoped<IAtencionService, AtencionService>();

builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IFinanzasService, FinanzasService>();
builder.Services.AddScoped<IComprobanteService, ComprobanteService>();

// --- SERVICIOS MÓDULOS OBSTÉTRICOS (ACCESO DIRECTO A CONTEXTO) ---
// Nota: Si Anamnesis comparte esta estructura directa, se inyecta igual que los demás:
// builder.Services.AddScoped<IAnamnesisService, AnamnesisService>(); 
builder.Services.AddScoped<IExamenFisicoService, ExamenFisicoService>();
builder.Services.AddScoped<ITactoVaginalService, TactoVaginalService>();
builder.Services.AddScoped<IEcografiaObstetricaService, EcografiaObstetricaService>();
builder.Services.AddScoped<IImpresionDiagnosticaService, ImpresionDiagnosticaService>();

// --- UTILERÍAS PURAS DE GENERACIÓN DE DOCUMENTOS PDF (QUESTPDF) ---
builder.Services.AddScoped<IComprobantePdfService, ComprobantePdfService>();
builder.Services.AddScoped<IHistoriaClinicaPdfService, HistoriaClinicaPdfService>();
builder.Services.AddScoped<IResumenPartoPdfService, ResumenPartoPdfService>();
builder.Services.AddScoped<IReporteFinancieroPdfService, ReporteFinancieroPdfService>();
builder.Services.AddScoped<ICertificadoTrabajoPdfService, CertificadoTrabajoPdfService>();

// ==========================================================
// INTEGRACIONES DE SERVICIOS EXTERNOS (NOTIFICACIONES WHATSAPP)
// ==========================================================
builder.Services.AddSignalR();
builder.Services.Configure<WhatsAppOptions>(builder.Configuration.GetSection("WhatsApp"));

builder.Services.AddHttpClient<INotificacionWhatsAppService, EvolutionWhatsAppService>((serviceProvider, client) =>
{
    var options = serviceProvider
        .GetRequiredService<Microsoft.Extensions.Options.IOptions<WhatsAppOptions>>()
        .Value;

    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Tarea en segundo plano para envío automático de recordatorios médicos
builder.Services.AddHostedService<RecordatorioCitasBackgroundService>();

// ==========================================================
// POLÍTICAS CORS RESTRINGIDAS
// ==========================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirBlazor", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7299",
                "http://localhost:5091",
                "https://salmon-bush-08c1e7510.7.azurestaticapps.net" 
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ==========================================================
// INICIALIZACIÓN DE LICENCIA QUESTPDF
// ==========================================================
QuestPDF.Settings.License = LicenseType.Community;

// ==========================================================
// PIPELINE DE EJECUCIÓN DE LA APLICACIÓN (MIDDLEWARES)
// ==========================================================
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Middleware personalizado de Cabeceras de Seguridad Rigurosas (Mitigación OWASP ZAP)
app.Use(async (context, next) =>
{
    // 🚀 SI ES SWAGGER, no le aplicamos el CSP estricto para que el navegador no bloquee su interfaz
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        await next();
        return;
    }

    // Generar un nonce aleatorio para el resto de la aplicación (Blazor, páginas, etc.)
    var nonce = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
        .Replace("+", "-").Replace("/", "_").Replace("=", "");

    context.Items["CspNonce"] = nonce;

    // Cabeceras fijas de protección
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

    // CSP con nonce y strict-dynamic
    context.Response.Headers.Append("Content-Security-Policy",
        $"default-src 'self'; " +
        $"script-src 'self' 'nonce-{nonce}' 'strict-dynamic'; " +
        $"style-src 'self' 'nonce-{nonce}'; " +
        $"frame-ancestors 'none'; " +
        $"object-src 'none'; " +
        $"base-uri 'self';");

    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Psicomedix API v1");
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("PermitirBlazor");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

// ==========================================================
// EJECUCIÓN DEL SEEDER DE BASE DE DATOS AL ARRANCAR
// ==========================================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Lógica del DataSeeder adaptada al contexto del monolito unificado
    // await DataSeeder.SeedAsync(context);
}

app.Run();

namespace psicomedixMonolito
{
    public partial class Program { }
}