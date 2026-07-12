using psicomedixMonolito.DTOs.Pacientes;

namespace psicomedixMonolito.Services;

public interface IPacienteService
{
    Task<IEnumerable<PacienteResponseDto>> ObtenerTodosAsync();
    Task<PacienteResponseDto?> ObtenerPorIdAsync(Guid id);
    Task<PacienteResponseDto?> ObtenerPorDniAsync(string dni);
    
    // Procesa el DTO unificado con soporte para nulos y la clave foránea hacia Doctor
    Task<Guid> CrearAsync(CrearPacienteDto dto, Guid usuarioId);
    
    Task ActualizarContactoAsync(Guid id, ActualizarContactoPacienteDto dto);
    Task CambiarEstadoAsync(Guid id, CambiarEstadoPacienteDto dto);
}