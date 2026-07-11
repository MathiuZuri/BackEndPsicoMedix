using psicomedixMonolito.DTOs.Roles;

namespace psicomedixMonolito.Services;

public interface IRolService
{
    Task<IEnumerable<RolResponseDto>> ObtenerTodosAsync();
    Task<RolResponseDto?> ObtenerPorIdAsync(Guid id);
    Task<Guid> CrearAsync(CrearRolDto dto);
    Task ActualizarAsync(Guid id, EditarRolDto dto);
    Task AsignarPermisosAsync(AsignarPermisosRolDto dto);
}