using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Servicios;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la consulta del catálogo de servicios clínicos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Servicios Clínicos")]
public class ServiciosClinicosController : ControllerBase
{
    private readonly IServicioClinicoService _servicioService;

    public ServiciosClinicosController(IServicioClinicoService servicioService)
    {
        _servicioService = servicioService;
    }

    /// <summary>
    /// Obtiene la lista completa de todos los servicios clínicos registrados.
    /// </summary>
    /// <remarks>
    /// **Uso:** Consulta el catálogo completo de servicios, incluyendo costo base, duración y requisitos.
    /// Útil para la programación de citas y facturación.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ServicioVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.ServicioVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ServicioClinicoResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var servicios = await _servicioService.ObtenerTodosAsync();
        return Ok(ApiResponse<object>.Ok(servicios, "Servicios clínicos obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene la lista de servicios clínicos activos.
    /// </summary>
    /// <remarks>
    /// **Uso:** Filtra solo los servicios con estado "Activo", que son los que pueden seleccionarse para citas.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ServicioVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.ServicioVer)]
    [HttpGet("activos")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ServicioClinicoResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetActivos()
    {
        var servicios = await _servicioService.ObtenerActivosAsync();
        return Ok(ApiResponse<object>.Ok(servicios, "Servicios clínicos activos obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene los datos de un servicio clínico específico por su ID.
    /// </summary>
    /// <remarks>
    /// **Uso:** Recupera la información completa de un servicio clínico individual.
    /// Útil para la pantalla de detalle o para validar datos antes de una programación.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ServicioVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.ServicioVer)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServicioClinicoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var servicio = await _servicioService.ObtenerPorIdAsync(id);
        if (servicio == null)
            throw new KeyNotFoundException("Servicio clínico no encontrado.");
        return Ok(ApiResponse<object>.Ok(servicio, "Servicio clínico obtenido correctamente."));
    }
}