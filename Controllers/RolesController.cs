using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Roles;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión de roles y la asignación de permisos en el sistema.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Seguridad - Roles")]
public class RolesController : ControllerBase
{
    private readonly IRolService _rolService;

    public RolesController(IRolService rolService)
    {
        _rolService = rolService;
    }

    /// <summary>
    /// Obtiene la lista de todos los roles del sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el catálogo completo de roles, incluyendo los del sistema y los creados por el usuario.
    /// Útil para la administración de seguridad y la asignación de roles a usuarios.
    /// **Permiso requerido:** <see cref="PermisosPolicies.RolVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.RolVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RolResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _rolService.ObtenerTodosAsync();
        return Ok(ApiResponse<object>.Ok(roles, "Roles obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene los detalles de un rol específico por su ID.
    /// </summary>
    /// <remarks>
    /// **Uso:** Recupera la información completa de un rol individual.
    /// Útil para la pantalla de detalle del rol o para validar datos antes de una actualización.
    /// **Permiso requerido:** <see cref="PermisosPolicies.RolVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.RolVer)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RolResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var rol = await _rolService.ObtenerPorIdAsync(id);
        if (rol == null)
            throw new KeyNotFoundException("Rol no encontrado.");
        return Ok(ApiResponse<object>.Ok(rol, "Rol obtenido correctamente."));
    }

    /// <summary>
    /// Crea un nuevo rol en el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Registra un nuevo rol con nombre y descripción. No tiene permisos asignados por defecto.
    /// **Permiso requerido:** <see cref="PermisosPolicies.RolCrear"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.RolCrear)]
    [Auditoria("Roles", "Rol", TipoAccionAuditoria.Creacion, NivelAuditoria.Critico)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CrearRolDto dto)
    {
        var id = await _rolService.CrearAsync(dto);
        return CreatedAtAction(
            nameof(GetById),
            new { id },
            ApiResponse<object>.Ok(new { id }, "Rol creado correctamente.", 201)
        );
    }

    /// <summary>
    /// Actualiza los datos de un rol existente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Modifica el nombre, la descripción o el estado de un rol.
    /// **Permiso requerido:** <see cref="PermisosPolicies.RolEditar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.RolEditar)]
    [Auditoria("Roles", "Rol", TipoAccionAuditoria.Edicion, NivelAuditoria.Critico)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] EditarRolDto dto)
    {
        await _rolService.ActualizarAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Rol actualizado correctamente."));
    }

    /// <summary>
    /// Asigna permisos a un rol específico.
    /// </summary>
    /// <remarks>
    /// **Uso:** Otorga una lista de permisos a un rol. Los permisos que ya tenga se ignoran.
    /// **Permiso requerido:** <see cref="PermisosPolicies.RolAsignarPermisos"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.RolAsignarPermisos)]
    [Auditoria("Roles", "RolPermiso", TipoAccionAuditoria.Asignacion, NivelAuditoria.Critico)]
    [HttpPost("asignar-permisos")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignPermissions([FromBody] AsignarPermisosRolDto dto)
    {
        await _rolService.AsignarPermisosAsync(dto);
        return Ok(ApiResponse<object>.Ok(dto, "Permisos asignados correctamente."));
    }
}