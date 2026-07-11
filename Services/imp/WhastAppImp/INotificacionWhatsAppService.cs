namespace psicomedixMonolito.Services.imp.WhastAppImp;

// esto es exclusivo de evolution api
public interface INotificacionWhatsAppService
{
    Task EnviarMensajeAsync(string telefonoDestino, string mensaje, CancellationToken cancellationToken = default);
    Task<string?> ObtenerQrInstanciaAsync(CancellationToken cancellationToken = default);
    Task<string?> BuscarContactoEnAgendaAsync(string jidLid, CancellationToken cancellationToken = default);
}