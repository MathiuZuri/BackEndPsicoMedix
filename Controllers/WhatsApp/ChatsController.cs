using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.WhatsAppDto;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services.imp.WhastAppImp;
using psicomedixMonolito.Utils.Hubs;

namespace psicomedixMonolito.Controllers.WhatsApp;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Asegura que solo usuarios logueados (secretarias/psicólogos) usen el chat
public class ChatsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly INotificacionWhatsAppService _whatsAppService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ILogger<ChatsController> _logger;

    public ChatsController(
        ApplicationDbContext context,
        INotificacionWhatsAppService whatsAppService,
        IHubContext<ChatHub> hubContext,
        ILogger<ChatsController> logger)
    {
        _context = context;
        _whatsAppService = whatsAppService;
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Envía un mensaje de WhatsApp a un paciente desde el panel de la clínica.
    /// </summary>
    [HttpPost("enviar")]
    public async Task<IActionResult> EnviarMensaje([FromBody] EnviarMensajeDto dto)
    {
        // 1. Buscar el chat en la Base de Datos
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == dto.ChatId);
        if (chat == null) return NotFound(new { mensaje = "El chat solicitado no existe." });

        // Al haber corregido el Webhook, aquí siempre vendrá el número real (@s.whatsapp.net)
        string telefonoDestino = chat.TelefonoWhatsApp;

        try
        {
            // 2. Despachar el mensaje directamente a través de Evolution API
            await _whatsAppService.EnviarMensajeAsync(telefonoDestino, dto.Texto);

            // 3. Guardar la burbuja de texto saliente en tu Postgres local
            var nuevoMensaje = new MensajeChat
            {
                Id = Guid.NewGuid(),
                ChatId = chat.Id,
                Texto = dto.Texto,
                EsMio = true,
                FechaEnvio = DateTime.UtcNow
            };

            _context.MensajesChat.Add(nuevoMensaje);
            
            chat.UltimoMensaje = dto.Texto;
            chat.FechaUltimaInteraccion = DateTime.UtcNow;
            _context.Chats.Update(chat);

            await _context.SaveChangesAsync();

            // 4. Emitir por SignalR para actualizar la pantalla de Blazor instantáneamente
            var payloadSignalR = new
            {
                ChatId = chat.Id,
                Telefono = chat.TelefonoWhatsApp,
                Nombre = chat.NombreContacto,
                Texto = nuevoMensaje.Texto,
                EsMio = true,
                Fecha = nuevoMensaje.FechaEnvio.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                MensajesNoLeidos = chat.MensajesNoLeidos
            };

            await _hubContext.Clients.All.SendAsync("RecibirNuevoMensaje", payloadSignalR);

            return Ok(new { status = "delivered" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al intentar enviar mensaje de WhatsApp al chat {ChatId}", dto.ChatId);
            return StatusCode(500, new { mensaje = "Error interno al procesar el envío a través de WhatsApp." });
        }
    }

    /// <summary>
    /// Endpoint complementario para listar las conversaciones activas (Columna izquierda del WhatsApp clon)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerChatsActivos()
    {
        var chats = await _context.Chats
            .OrderByDescending(c => c.FechaUltimaInteraccion)
            .ToListAsync();

        return Ok(chats);
    }

    /// <summary>
    /// Endpoint complementario para cargar las burbujas de un chat específico al hacer clic en él
    /// </summary>
    [HttpGet("{chatId:guid}/mensajes")]
    public async Task<IActionResult> ObtenerHistorialMensajes(Guid chatId)
    {
        var mensajes = await _context.MensajesChat
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.FechaEnvio)
            .ToListAsync();

        return Ok(mensajes);
    }
}