using psicomedixMonolito.DTOs.Atenciones;

namespace psicomedixMonolito.Services.ATENCIONES;

public interface IAtencionService
{
    Task<IEnumerable<AtencionResponseDto>> ObtenerTodasAsync();
    Task<IEnumerable<AtencionResponseDto>> ObtenerPorPacienteAsync(Guid pacienteId);
    Task<AtencionResponseDto?> ObtenerPorIdAsync(Guid id);
    Task<Guid> RegistrarAtencionAsync(RegistrarAtencionDto dto, Guid usuarioId);
    Task CerrarAtencionAsync(Guid id, CerrarAtencionDto dto);
    Task AnularAtencionAsync(Guid id, string motivo);
}