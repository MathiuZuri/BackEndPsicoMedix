using psicomedixMonolito.DTOs.Pagos;

namespace psicomedixMonolito.Services;

public interface IPagoService
{
    Task<IEnumerable<PagoResponseDto>> ObtenerPorPacienteAsync(Guid pacienteId);
    Task<IEnumerable<PagoResponseDto>> ObtenerPorCitaAsync(Guid citaId);
    Task<IEnumerable<PagoResponseDto>> ObtenerPorAtencionAsync(Guid atencionId);
    Task<Guid> RegistrarAsync(RegistrarPagoDto dto, Guid usuarioId);  // 🔁 firma modificada
    Task CambiarEstadoAsync(Guid id, CambiarEstadoPagoDto dto);
}