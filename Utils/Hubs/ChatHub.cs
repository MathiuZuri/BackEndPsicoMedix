using Microsoft.AspNetCore.SignalR;

namespace psicomedixMonolito.Utils.Hubs;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(ILogger<ChatHub> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Evento que se ejecuta automáticamente cuando una pantalla de MudBlazor se conecta.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Cliente conectado al ChatHub: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Evento que se ejecuta cuando una pantalla se cierra o pierde internet.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Cliente desconectado del ChatHub: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Método opcional por si el Frontend quiere enviar un mensaje a través del Socket 
    /// (aunque lo ideal para enviar sigue siendo usar HTTP comercial).
    /// </summary>
    public async Task EnviarMensajeAClientes(object mensajePayload)
    {
        // Esto difunde el objeto JSON a absolutamente todos los clientes conectados en tiempo real
        await Clients.All.SendAsync("RecibirNuevoMensaje", mensajePayload);
    }
}