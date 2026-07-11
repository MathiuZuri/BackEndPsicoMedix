using psicomedixMonolito.DTOs.Servicios;

namespace psicomedixMonolito.Services;

public interface IServicioClinicoService
{
    Task<IEnumerable<ServicioClinicoResponseDto>> ObtenerTodosAsync();
    Task<IEnumerable<ServicioClinicoResponseDto>> ObtenerActivosAsync();
    Task<ServicioClinicoResponseDto?> ObtenerPorIdAsync(Guid id);
}