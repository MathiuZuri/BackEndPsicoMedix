using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Pagos;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión de pagos y transacciones financieras.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Gestión de Pagos")]
public class PagosController : ControllerBase
{
    private readonly IPagoService _pagoService;

    public PagosController(IPagoService pagoService)
    {
        _pagoService = pagoService;
    }

    /// <summary>
    /// Obtiene todos los pagos asociados a un paciente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Consulta el historial financiero de un paciente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PagoVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PagoVer)]
    [HttpGet("paciente/{pacienteId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PagoResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByPaciente(Guid pacienteId)
    {
        var pagos = await _pagoService.ObtenerPorPacienteAsync(pacienteId);
        return Ok(ApiResponse<IEnumerable<PagoResponseDto>>.Ok(pagos, "Pagos del paciente obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene todos los pagos asociados a una cita.
    /// </summary>
    /// <remarks>
    /// **Uso:** Consulta los pagos realizados en el contexto de una cita.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PagoVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PagoVer)]
    [HttpGet("cita/{citaId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PagoResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByCita(Guid citaId)
    {
        var pagos = await _pagoService.ObtenerPorCitaAsync(citaId);
        return Ok(ApiResponse<object>.Ok(pagos, "Pagos de la cita obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene todos los pagos asociados a una atención médica.
    /// </summary>
    /// <remarks>
    /// **Uso:** Consulta los pagos realizados en el contexto de una atención.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PagoVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PagoVer)]
    [HttpGet("atencion/{atencionId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PagoResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByAtencion(Guid atencionId)
    {
        var pagos = await _pagoService.ObtenerPorAtencionAsync(atencionId);
        return Ok(ApiResponse<object>.Ok(pagos, "Pagos de la atención obtenidos correctamente."));
    }

    /// <summary>
    /// Registra un nuevo pago.
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea un pago asociado a un paciente y servicio, calculando el saldo pendiente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PagoRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PagoRegistrar)]
    [Auditoria("Pagos", "Pago", TipoAccionAuditoria.Creacion, NivelAuditoria.Critico)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Registrar([FromBody] RegistrarPagoDto dto)
    {
        // 🚀 Ajuste Monolítico: Extraemos el ID del usuario actual desde el Token JWT
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                      User.FindFirst("sub")?.Value ??
                      User.FindFirst("usuarioId")?.Value ??
                      User.FindFirst("UsuarioId")?.Value;

        if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out var usuarioId))
        {
            return Unauthorized(new { mensaje = "No se pudo identificar al usuario autenticado en la sesión activa." });
        }

        // Se envía el DTO y el usuarioId para registrar el cobro y la traza de caja en el expediente
        var id = await _pagoService.RegistrarAsync(dto, usuarioId);
    
        return Ok(ApiResponse<object>.Ok(new { id }, "Pago registrado correctamente."));
    }

    /// <summary>
    /// Cambia el estado de un pago existente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Modifica el estado de un pago (por ejemplo, a "Anulado" o "Eliminado").
    /// **Permiso requerido:** <see cref="PermisosPolicies.PagoRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PagoRegistrar)]
    [Auditoria("Pagos", "Pago", TipoAccionAuditoria.Edicion, NivelAuditoria.Critico)]
    [HttpPut("{id:guid}/estado")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] CambiarEstadoPagoDto dto)
    {
        await _pagoService.CambiarEstadoAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Estado del pago actualizado correctamente."));
    }
}