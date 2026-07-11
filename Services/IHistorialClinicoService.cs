using psicomedixMonolito.DTOs.Historiales;

namespace psicomedixMonolito.Services;

public interface IHistorialClinicoService
{
    Task<HistorialClinicoResponseDto?> ObtenerPorPacienteAsync(Guid pacienteId);
    Task<HistorialClinicoResponseDto?> ObtenerConDetallesAsync(Guid historialId);
}