using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Historiales;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la consulta de historiales clínicos de pacientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Historial Clínico")]
public class HistorialesController : ControllerBase
{
    private readonly IHistorialClinicoService _historialService;

    public HistorialesController(IHistorialClinicoService historialService)
    {
        _historialService = historialService;
    }

    /// <summary>
    /// Obtiene el historial clínico completo de un paciente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite recuperar el resumen del historial clínico de un paciente,
    /// incluyendo sus datos básicos. Para obtener los detalles de los movimientos,
    /// use el endpoint <see cref="GetConDetalles"/>.
    /// **Permiso requerido:** <see cref="PermisosPolicies.HistorialVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.HistorialVer)]
    [HttpGet("paciente/{pacienteId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<HistorialClinicoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPaciente(Guid pacienteId)
    {
        var historial = await _historialService.ObtenerPorPacienteAsync(pacienteId);
        if (historial == null)
            throw new KeyNotFoundException("Historial clínico no encontrado.");
        return Ok(ApiResponse<object>.Ok(historial, "Historial clínico obtenido correctamente."));
    }

    /// <summary>
    /// Obtiene el historial clínico de un paciente con todos sus detalles (movimientos).
    /// </summary>
    /// <remarks>
    /// **Uso:** Proporciona la versión completa del historial clínico, incluyendo
    /// la lista detallada de todos los movimientos (citas, atenciones, pagos).
    /// Útil para la visualización completa de la línea de tiempo clínica.
    /// **Permiso requerido:** <see cref="PermisosPolicies.HistorialVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.HistorialVer)]
    [HttpGet("{historialId:guid}/detalles")]
    [ProducesResponseType(typeof(ApiResponse<HistorialClinicoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConDetalles(Guid historialId)
    {
        var historial = await _historialService.ObtenerConDetallesAsync(historialId);
        if (historial == null)
            throw new KeyNotFoundException("Historial clínico no encontrado.");
        return Ok(ApiResponse<object>.Ok(historial, "Historial clínico con detalles obtenido correctamente."));
    }
}