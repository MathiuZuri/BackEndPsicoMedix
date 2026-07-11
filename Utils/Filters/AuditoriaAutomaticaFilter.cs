using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Utils.Filters;

public class AuditoriaAutomaticaFilter : IAsyncActionFilter
{
    private readonly ApplicationDbContext _context;

    public AuditoriaAutomaticaFilter(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var http = context.HttpContext;
        var endpoint = context.ActionDescriptor as ControllerActionDescriptor;
        var metodo = http.Request.Method.ToUpper();

        var atributo = endpoint?.MethodInfo
            .GetCustomAttributes(typeof(AuditoriaAttribute), true)
            .OfType<AuditoriaAttribute>()
            .FirstOrDefault();

        var entidadIdAntes = ObtenerEntidadId(context);

        string? valorAnterior = null;

        if ((metodo is "PUT" or "PATCH" or "DELETE") && entidadIdAntes.HasValue)
        {
            valorAnterior = await ObtenerSnapshotEntidadAsync(
                endpoint?.ControllerName,
                entidadIdAntes.Value
            );
        }

        var ejecutado = await next();

        var usuarioIdDesdeRespuesta = ObtenerUsuarioIdDesdeResultado(ejecutado);

        var usuarioIdClaim = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        Guid? usuarioId = Guid.TryParse(usuarioIdClaim, out var idUsuario)
            ? idUsuario
            : usuarioIdDesdeRespuesta;

        var entidadIdDesdeRespuesta = ObtenerEntidadIdDesdeResultado(ejecutado);

        var entidadIdFinal = entidadIdAntes ?? entidadIdDesdeRespuesta;

        var statusCode = http.Response.StatusCode;

        var fueExitoso = statusCode >= 200 && statusCode < 400 && ejecutado.Exception == null;

        string? valorNuevo = null;

        if (metodo is "POST" or "PUT" or "PATCH")
        {
            if (entidadIdFinal.HasValue)
            {
                valorNuevo = await ObtenerSnapshotEntidadAsync(
                    endpoint?.ControllerName,
                    entidadIdFinal.Value
                );
            }
        }

        valorNuevo ??= JsonSerializer.Serialize(new
        {
            Metodo = http.Request.Method,
            Ruta = http.Request.Path.Value,
            Query = http.Request.QueryString.Value,
            StatusCode = statusCode,
            Accion = endpoint?.ActionName,
            Controlador = endpoint?.ControllerName
        });

        // ✅ Indicador de consulta
        var esConsulta = metodo == "GET";

        // ✅ IP real (maneja proxies)
        var ipReal = ObtenerIpReal(http);

        var auditoria = new Auditoria
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuarioId,
            TipoAccion = atributo?.TipoAccion ?? DetectarTipoAccion(http.Request.Method),
            Nivel = atributo?.Nivel ?? DetectarNivel(http.Request.Method),
            Modulo = atributo?.Modulo ?? endpoint?.ControllerName ?? "Sistema",
            EntidadAfectada = atributo?.Entidad ?? endpoint?.ControllerName ?? "General",
            EntidadId = entidadIdFinal,
            Descripcion = GenerarDescripcion(http.Request.Method, endpoint, fueExitoso, statusCode),
            ValorAnterior = valorAnterior,
            ValorNuevo = valorNuevo,
            IpAddress = ipReal,
            UserAgent = http.Request.Headers.UserAgent.ToString(),
            FueExitoso = fueExitoso,
            DetalleError = ejecutado.Exception?.Message,
            FechaHora = DateTime.UtcNow,
            EsConsulta = esConsulta,
        };

        _context.Auditorias.Add(auditoria);
        await _context.SaveChangesAsync();
    }

    // ✅ Método para obtener IP real
    private string? ObtenerIpReal(HttpContext httpContext)
    {
        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
            return forwardedFor.Split(',').First().Trim();

        return httpContext.Connection.RemoteIpAddress?.ToString();
    }

    // ---------- El resto de métodos privados se mantienen igual ----------
    private static TipoAccionAuditoria DetectarTipoAccion(string metodo)
    {
        return metodo.ToUpper() switch
        {
            "GET" => TipoAccionAuditoria.Consulta,
            "POST" => TipoAccionAuditoria.Creacion,
            "PUT" => TipoAccionAuditoria.Edicion,
            "PATCH" => TipoAccionAuditoria.Edicion,
            "DELETE" => TipoAccionAuditoria.Eliminacion,
            _ => TipoAccionAuditoria.Consulta
        };
    }

    private static NivelAuditoria DetectarNivel(string metodo)
    {
        return metodo.ToUpper() switch
        {
            "POST" => NivelAuditoria.Importante,
            "PUT" => NivelAuditoria.Importante,
            "PATCH" => NivelAuditoria.Importante,
            "DELETE" => NivelAuditoria.Critico,
            _ => NivelAuditoria.Normal
        };
    }

    private static Guid? ObtenerEntidadId(ActionExecutingContext context)
    {
        string? valor = null;

        if (context.RouteData.Values.TryGetValue("id", out var id))
            valor = id?.ToString();
        else if (context.RouteData.Values.TryGetValue("comprobanteId", out var comprobanteId))
            valor = comprobanteId?.ToString();
        else if (context.RouteData.Values.TryGetValue("pagoId", out var pagoId))
            valor = pagoId?.ToString();
        else if (context.RouteData.Values.TryGetValue("usuarioId", out var usuarioId))
            valor = usuarioId?.ToString();
        else if (context.RouteData.Values.TryGetValue("pacienteId", out var pacienteId))
            valor = pacienteId?.ToString();
        else if (context.RouteData.Values.TryGetValue("doctorId", out var doctorId))
            valor = doctorId?.ToString();
        else if (context.RouteData.Values.TryGetValue("citaId", out var citaId))
            valor = citaId?.ToString();
        else if (context.RouteData.Values.TryGetValue("atencionId", out var atencionId))
            valor = atencionId?.ToString();
        else if (context.RouteData.Values.TryGetValue("historialId", out var historialId))
            valor = historialId?.ToString();
        else if (context.RouteData.Values.TryGetValue("historialClinicoId", out var historialClinicoId))
            valor = historialClinicoId?.ToString();

        return Guid.TryParse(valor, out var guid) ? guid : null;
    }

    private static Guid? ObtenerUsuarioIdDesdeResultado(ActionExecutedContext ejecutado)
    {
        if (ejecutado.Result is not ObjectResult objectResult)
            return null;

        return BuscarGuidPorNombre(objectResult.Value, "UsuarioId");
    }

    private static Guid? ObtenerEntidadIdDesdeResultado(ActionExecutedContext ejecutado)
    {
        if (ejecutado.Result is not ObjectResult objectResult)
            return null;

        return BuscarGuidPorNombre(objectResult.Value, "id");
    }

    private static Guid? BuscarGuidPorNombre(object? objeto, string nombrePropiedad)
    {
        if (objeto == null)
            return null;

        var tipo = objeto.GetType();

        var propiedadDirecta = tipo.GetProperties()
            .FirstOrDefault(p => string.Equals(
                p.Name,
                nombrePropiedad,
                StringComparison.OrdinalIgnoreCase));

        if (propiedadDirecta != null)
        {
            var valor = propiedadDirecta.GetValue(objeto);

            if (valor is Guid guid)
                return guid;

            if (Guid.TryParse(valor?.ToString(), out var guidParseado))
                return guidParseado;
        }

        var propiedadData = tipo.GetProperties()
            .FirstOrDefault(p => string.Equals(
                p.Name,
                "Data",
                StringComparison.OrdinalIgnoreCase));

        if (propiedadData == null)
            return null;

        var data = propiedadData.GetValue(objeto);
        if (data == null)
            return null;

        return BuscarGuidPorNombre(data, nombrePropiedad);
    }

    private async Task<string?> ObtenerSnapshotEntidadAsync(string? controllerName, Guid id)
    {
        object? entidad = controllerName switch
        {
            "Pacientes" => await _context.Pacientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Doctores" => await _context.Doctores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Horarios" => await _context.HorariosDoctor.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Citas" => await _context.Citas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Atenciones" => await _context.Atenciones.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Pagos" => await _context.Pagos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Comprobantes" => await _context.Comprobantes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Usuarios" => await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Roles" => await _context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "ServiciosClinicos" => await _context.ServiciosClinicos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            "Historiales" => await _context.HistorialesClinicos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id),
            _ => null
        };

        if (entidad == null)
            return null;

        var snapshot = ConvertirASnapshotPlano(entidad);

        return JsonSerializer.Serialize(snapshot, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private static Dictionary<string, object?> ConvertirASnapshotPlano(object entidad)
    {
        var resultado = new Dictionary<string, object?>();

        var propiedades = entidad.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => EsPropiedadSimple(p.PropertyType));

        foreach (var propiedad in propiedades)
        {
            resultado[propiedad.Name] = propiedad.GetValue(entidad);
        }

        return resultado;
    }

    private static bool EsPropiedadSimple(Type type)
    {
        var tipoReal = Nullable.GetUnderlyingType(type) ?? type;

        return tipoReal.IsPrimitive
               || tipoReal.IsEnum
               || tipoReal == typeof(string)
               || tipoReal == typeof(decimal)
               || tipoReal == typeof(Guid)
               || tipoReal == typeof(DateTime)
               || tipoReal == typeof(DateOnly)
               || tipoReal == typeof(TimeOnly);
    }

    private static string GenerarDescripcion(
        string metodo,
        ControllerActionDescriptor? endpoint,
        bool fueExitoso,
        int statusCode)
    {
        var resultado = fueExitoso ? "exitosa" : "fallida";

        return $"Solicitud {metodo} ejecutada en {endpoint?.ControllerName}/{endpoint?.ActionName}. Resultado: {resultado}. Código HTTP: {statusCode}.";
    }
}