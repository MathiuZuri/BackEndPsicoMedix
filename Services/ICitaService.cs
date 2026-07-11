using psicomedixMonolito.DTOs.Citas;

namespace psicomedixMonolito.Services;

public interface ICitaService
{
    Task<IEnumerable<CitaResponseDto>> ObtenerTodasAsync();
    Task<CitaResponseDto?> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<CitaResponseDto>> ObtenerPorPacienteAsync(Guid pacienteId);
    Task<IEnumerable<CitaResponseDto>> ObtenerPorDoctorAsync(Guid doctorId);
    
    // Pasamos el usuarioId directamente desde el controlador
    Task<Guid> CrearAsync(CrearCitaDto dto, Guid usuarioId);
    Task ReprogramarAsync(Guid id, ReprogramarCitaDto dto);
    Task CancelarAsync(Guid id, CancelarCitaDto dto);
}