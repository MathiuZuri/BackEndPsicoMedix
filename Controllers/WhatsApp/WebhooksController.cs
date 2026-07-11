using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.WhatsAppDto;
using psicomedixMonolito.Models;
using psicomedixMonolito.Utils.Configurations;
using psicomedixMonolito.Utils.Hubs;
// Asegura que este using apunte a donde está tu interfaz
using INotificacionWhatsAppService = psicomedixMonolito.Services.imp.WhastAppImp.INotificacionWhatsAppService;

namespace psicomedixMonolito.Controllers.WhatsApp;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class WebhooksController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly WhatsAppOptions _whatsAppOptions;
    private readonly ILogger<WebhooksController> _logger;
    private readonly INotificacionWhatsAppService _whatsAppService;

    public WebhooksController(
        ApplicationDbContext context,
        IHubContext<ChatHub> hubContext,
        IOptions<WhatsAppOptions> whatsAppOptions,
        INotificacionWhatsAppService whatsAppService, 
        ILogger<WebhooksController> logger)
    {
        _context = context;
        _hubContext = hubContext;
        _whatsAppOptions = whatsAppOptions.Value;
        _whatsAppService = whatsAppService;
        _logger = logger;
    }

    [HttpPost("whatsapp/{*eventType}")]
    public async Task<IActionResult> ReceiveWhatsAppEvent(
        [FromHeader(Name = "apikey")] string? incomingApiKey,
        string? eventType)
    {
        string tokenConfigurado = _whatsAppOptions.WebhookSecretToken;
        string tokenInstanciaGrafica = "9348250EBE7B-49F9-9A6B-E0A8C079C58D";

        bool esLlaveValida = !string.IsNullOrEmpty(incomingApiKey) && 
                             (incomingApiKey == tokenConfigurado || incomingApiKey == tokenInstanciaGrafica);

    #if DEBUG
        if (!esLlaveValida) esLlaveValida = true;
    #endif

        if (!esLlaveValida) return Ok();

        using var reader = new StreamReader(Request.Body);
        string rawJson = await reader.ReadToEndAsync();
        if (string.IsNullOrWhiteSpace(rawJson)) return Ok();

        using var doc = JsonDocument.Parse(rawJson);
        if (!doc.RootElement.TryGetProperty("event", out var eventProp) || eventProp.GetString() != "messages.upsert")
        {
            return Ok();
        }

        EvolutionWebhookDto? payload;
        try
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            payload = JsonSerializer.Deserialize<EvolutionWebhookDto>(rawJson, options);
            if (payload == null || payload.Data == null) return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al deserializar el payload");
            return Ok(); 
        }

        var keyData = payload.Data.Key;
        var textoMensaje = payload.Data.Message?.Conversation 
                           ?? payload.Data.Message?.ExtendedTextMessage?.Text;

        if (string.IsNullOrEmpty(textoMensaje)) return Ok();

        // 🌟 PASO A: Priorizar el JID alternativo (número real) que v2.3.7 resuelve de forma nativa
        var jidDestinoCompleto = !string.IsNullOrEmpty(keyData.RemoteJidAlt) 
            ? keyData.RemoteJidAlt 
            : keyData.RemoteJid; 

        bool esMensajeMio = keyData.FromMe;

        // PASO B: Si sigue siendo un @lid puro sin JidAlt y no es mío, usamos tu servicio de agenda
        if (jidDestinoCompleto.Contains("@lid") && !esMensajeMio)
        {
            _logger.LogInformation("[SantaMonica Webhook] Detectado @lid puro sin JidAlt. Consultando libreta del celular...");
        
            var numeroRealDescubierto = await _whatsAppService.BuscarContactoEnAgendaAsync(jidDestinoCompleto);
        
            if (!string.IsNullOrEmpty(numeroRealDescubierto))
            {
                jidDestinoCompleto = numeroRealDescubierto; 
            }
        }

        // Evitar procesar mensajes duplicados que reintente la cola de Docker
        var yaExisteMensaje = await _context.MensajesChat.AnyAsync(m => m.MessageIdWhatsApp == keyData.Id);
        if (yaExisteMensaje) return Ok(new { status = "duplicate_ignored" });

        // Buscar si el chat ya existe con el JID desempatado
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.TelefonoWhatsApp == jidDestinoCompleto);

        if (chat == null)
        {
            // CASO NUEVO: Crear un hilo de conversación independiente
            chat = new Chat
            {
                TelefonoWhatsApp = jidDestinoCompleto, 
                NombreContacto = payload.Data.PushName ?? "Contacto de WhatsApp",
                UltimoMensaje = textoMensaje,
                FechaUltimaInteraccion = DateTime.UtcNow,
                MensajesNoLeidos = esMensajeMio ? 0 : 1
            };

            // Vinculación automática inteligente con la tabla de Pacientes
            var soloNumeros = new string(jidDestinoCompleto.Split('@')[0].Where(char.IsDigit).ToArray());
            if (soloNumeros.Length >= 9)
            {
                var ultimosNueveDigitos = soloNumeros.Substring(soloNumeros.Length - 9);
                var paciente = await _context.Pacientes
                    .FirstOrDefaultAsync(p => p.Celular != null && p.Celular.Contains(ultimosNueveDigitos));

                if (paciente != null)
                {
                    chat.PacienteId = paciente.Id;
                    chat.NombreContacto = $"{paciente.Nombres} {paciente.Apellidos}".Trim();
                }
            }

            _context.Chats.Add(chat);
        }
        else
        {
            // CASO EXISTENTE: Actualizar estados del chat actual
            chat.UltimoMensaje = textoMensaje;
            chat.FechaUltimaInteraccion = DateTime.UtcNow;
            if (!esMensajeMio) chat.MensajesNoLeidos++;

            // 💡 Actualización dinámica de nombre si antes era genérico y ahora viene el real
            if (chat.NombreContacto == "Contacto de WhatsApp" && !string.IsNullOrEmpty(payload.Data.PushName))
            {
                chat.NombreContacto = payload.Data.PushName;
            }

            _context.Chats.Update(chat);
        }

        await _context.SaveChangesAsync();

        // Persistir la burbuja de texto vinculada al ID único del Chat
        var nuevoMensaje = new MensajeChat
        {
            ChatId = chat.Id,
            Texto = textoMensaje,
            EsMio = esMensajeMio,
            FechaEnvio = DateTime.UtcNow,
            MessageIdWhatsApp = keyData.Id
        };

        _context.MensajesChat.Add(nuevoMensaje);
        await _context.SaveChangesAsync();

        // Difusión en tiempo real hacia todas las pantallas de Blazor activas vía SignalR
        var payloadSignalR = new
        {
            ChatId = chat.Id,              
            Telefono = chat.TelefonoWhatsApp, 
            Nombre = chat.NombreContacto,    
            Texto = nuevoMensaje.Texto,      
            EsMio = nuevoMensaje.EsMio,      
            Fecha = nuevoMensaje.FechaEnvio.ToString("yyyy-MM-ddTHH:mm:ssZ"), 
            MensajesNoLeidos = chat.MensajesNoLeidos 
        };

        await _hubContext.Clients.All.SendAsync("RecibirNuevoMensaje", payloadSignalR);
        return Ok(new { status = "persisted_and_broadcasted" });
    }
}