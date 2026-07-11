using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Permisos;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Utils.Authorization;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para consultar el diccionario de permisos del sistema.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Seguridad - Permisos")]
public class PermisosController : ControllerBase
{
    private readonly IPermisoService _permisoService;

    public PermisosController(IPermisoService permisoService)
    {
        _permisoService = permisoService;
    }

    /// <summary>
    /// Obtiene la lista completa de todos los permisos del sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el catálogo de permisos disponibles, incluyendo código, módulo y estado.
    /// Útil para la administración de roles y asignación de permisos.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PermisoVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PermisoVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PermisoResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var permisos = await _permisoService.ObtenerTodosAsync();
        return Ok(ApiResponse<object>.Ok(permisos, "Permisos obtenidos correctamente."));
    }
}