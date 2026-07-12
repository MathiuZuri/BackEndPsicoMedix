using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Atenciones;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.ATENCIONES;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión del ciclo de vida de las atenciones médicas (Core).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Atenciones Médicas (Core)")]
public class AtencionesController : ControllerBase
{
    private readonly IAtencionService _atencionService;

    public AtencionesController(IAtencionService atencionService)
    {
        _atencionService = atencionService;
    }

    /// <summary>
    /// Obtiene la lista de todas las atenciones registradas en el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el historial completo de atenciones. Útil para administración y reportes.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AtencionResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerTodas()
    {
        var atenciones = await _atencionService.ObtenerTodasAsync();
        return Ok(ApiResponse<object>.Ok(atenciones, "Atenciones obtenidas correctamente."));
    }

    /// <summary>
    /// Obtiene el historial de atenciones de un paciente específico.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar todas las atenciones de un paciente para ver su historial médico.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionVer)]
    [HttpGet("paciente/{pacienteId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AtencionResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerPorPaciente(Guid pacienteId)
    {
        var atenciones = await _atencionService.ObtenerPorPacienteAsync(pacienteId);
        return Ok(ApiResponse<object>.Ok(atenciones, "Atenciones del paciente obtenidas correctamente."));
    }

    /// <summary>
    /// Obtiene los detalles completos de una atención (Core + Todos sus módulos clínicos).
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite obtener toda la información clínica de una atención específica, incluyendo anamnesis, exámenes, diagnósticos, etc.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionVer)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AtencionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(Guid id)
    {
        var atencion = await _atencionService.ObtenerPorIdAsync(id);
        
        if (atencion == null) 
            return NotFound(ApiResponse<object>.Error("Atención no encontrada.", 404));
        
        return Ok(ApiResponse<object>.Ok(atencion, "Detalle de atención obtenido correctamente."));
    }

    /// <summary>
    /// Registra una nueva atención médica (Apertura).
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea una nueva atención asociada a un paciente, doctor y servicio. Además, genera el pago correspondiente y registra en el historial.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionRegistrar)]
    [Auditoria("Atenciones", "Atencion", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Registrar([FromBody] RegistrarAtencionDto dto)
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                      User.FindFirst("sub")?.Value ??
                      User.FindFirst("usuarioId")?.Value ??
                      User.FindFirst("UsuarioId")?.Value;

        if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out var usuarioId))
        {
            return Unauthorized(new { mensaje = "No se pudo identificar al usuario autenticado en la sesión activa." });
        }

        // Enviamos el DTO junto con el usuarioId resuelto al servicio de atenciones
        var id = await _atencionService.RegistrarAtencionAsync(dto, usuarioId);
    
        return CreatedAtAction(
            nameof(ObtenerPorId), 
            new { id }, 
            ApiResponse<object>.Ok(new { Id = id }, "Atención registrada y aperturada correctamente.", 201)
        );
    }

    /// <summary>
    /// Cierra una atención médica abierta.
    /// </summary>
    /// <remarks>
    /// **Uso:** Finaliza una atención, registrando el diagnóstico final y las indicaciones. Una vez cerrada, no se pueden modificar sus módulos clínicos.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionCerrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionCerrar)]
    [Auditoria("Atenciones", "Atencion", TipoAccionAuditoria.Edicion, NivelAuditoria.Importante)]
    [HttpPut("{id:guid}/cerrar")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cerrar(Guid id, [FromBody] CerrarAtencionDto dto)
    {
        await _atencionService.CerrarAtencionAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { Id = id }, "Atención cerrada y diagnóstico guardado exitosamente."));
    }

    /// <summary>
    /// Anula (eliminación lógica) una atención médica.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite anular una atención que fue creada por error o que no se completó. La atención queda como "Anulada" y no se puede reactivar.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionCerrar"/> (o se puede crear uno específico como AtencionAnular).
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionCerrar)]
    [Auditoria("Atenciones", "Atencion", TipoAccionAuditoria.Eliminacion, NivelAuditoria.Critico)]
    [HttpPut("{id:guid}/anular")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Anular(Guid id, [FromBody] AnularAtencionRequest request)
    {
        await _atencionService.AnularAtencionAsync(id, request.Motivo);
        return Ok(ApiResponse<object>.Ok(new { Id = id }, "Atención anulada correctamente."));
    }
}

/// <summary>
/// Clase auxiliar para recibir el motivo de anulación.
/// </summary>
public class AnularAtencionRequest
{
    public string Motivo { get; set; } = string.Empty;
}