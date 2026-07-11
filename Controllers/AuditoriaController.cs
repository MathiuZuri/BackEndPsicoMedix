using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Auditoria;
using psicomedixMonolito.DTOs.Comunes;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;

namespace psicomedixMonolito.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Auditoría del Sistema")]
public class AuditoriaController : ControllerBase
{
    private readonly IAuditoriaService _auditoriaService;

    public AuditoriaController(IAuditoriaService auditoriaService)
    {
        _auditoriaService = auditoriaService;
    }

    /// <summary>
    /// Obtiene todos los registros de auditoría con paginación y filtros opcionales.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el historial completo de auditoría del sistema.
    /// Se puede filtrar por tipo de acción (Creación, Edición, etc.) o solo consultas.
    /// Útil para supervisión y cumplimiento normativo.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AuditoriaVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AuditoriaVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginacionResponseDto<AuditoriaResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginacionRequestDto request,
        [FromQuery] TipoAccionAuditoria? tipoAccion = null,
        [FromQuery] bool? soloConsultas = null)
    {
        var resultado = await _auditoriaService.ObtenerTodosPaginadosAsync(request, tipoAccion, soloConsultas);
        return Ok(ApiResponse<object>.Ok(resultado, "Registros de auditoría obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene los registros de auditoría de un usuario específico con paginación y filtros.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite filtrar los registros de auditoría por un usuario determinado.
    /// Útil para investigar la actividad de un usuario específico.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AuditoriaVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AuditoriaVer)]
    [HttpGet("usuario/{usuarioId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PaginacionResponseDto<AuditoriaResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByUsuario(
        Guid usuarioId,
        [FromQuery] PaginacionRequestDto request,
        [FromQuery] TipoAccionAuditoria? tipoAccion = null,
        [FromQuery] bool? soloConsultas = null)
    {
        var resultado = await _auditoriaService.ObtenerPorUsuarioPaginadosAsync(usuarioId, request, tipoAccion, soloConsultas);
        return Ok(ApiResponse<object>.Ok(resultado, "Registros de auditoría del usuario obtenidos correctamente."));
    }
}