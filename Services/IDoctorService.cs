using psicomedixMonolito.DTOs.Comunes;
using psicomedixMonolito.DTOs.Doctores;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Services;

public interface IDoctorService
{
    Task<IEnumerable<DoctorResponseDto>> ObtenerTodosAsync();
    Task<IEnumerable<DoctorResponseDto>> ObtenerActivosAsync();
    Task<DoctorResponseDto?> ObtenerPorIdAsync(Guid id);
    
    // Se añade el parámetro usuarioId para la traza de auditoría del registro
    Task<Guid> CrearAsync(CrearDoctorDto dto, Guid usuarioId);
    
    Task ActualizarAsync(Guid id, EditarDoctorDto dto);
    Task<Guid> ContratarAsync(ContratarDoctorDto dto);
    
    Task<PaginacionResponseDto<DoctorResponseDto>> BuscarAsync(
        string? nombre,
        string? especialidad,
        EstadoDoctor? estado,
        PaginacionRequestDto request);
}