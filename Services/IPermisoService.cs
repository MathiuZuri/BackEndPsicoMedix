using psicomedixMonolito.DTOs.Permisos;

namespace psicomedixMonolito.Services;

public interface IPermisoService
{
    Task<IEnumerable<PermisoResponseDto>> ObtenerTodosAsync();
}