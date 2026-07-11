using System;
using System.Collections.Generic;
using System.Security.Claims; // 🚀 Añadido para extraer Claims nativamente
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Comprobantes;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la emisión, generación de PDF y gestión de comprobantes (boletas, constancias, resúmenes, estados de cuenta).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Base: todos requieren autenticación
[Tags("Comprobantes y Documentos")]
public class ComprobantesController : ControllerBase
{
    private readonly IComprobanteService _comprobanteService;

    public ComprobantesController(IComprobanteService comprobanteService)
    {
        _comprobanteService = comprobanteService;
    }

    // ==========================================================
    // PREVIEWS (Vistas previas)
    // ==========================================================

    /// <summary>
    /// Genera una vista previa de la boleta de pago.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite visualizar el contenido de la boleta antes de emitirla.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteVer"/>.
    /// </remarks>
    [HttpGet("preview/boleta-pago/{pagoId:guid}")]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(ComprobantePagoPreviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComprobantePagoPreviewDto>> PreviewBoletaPago(Guid pagoId)
    {
        var resultado = await _comprobanteService.PreviewBoletaPagoAsync(pagoId);
        return Ok(resultado);
    }

    /// <summary>
    /// Genera una vista previa de la constancia de cita.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite visualizar el contenido de la constancia antes de emitirla.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteVer"/>.
    /// </remarks>
    [HttpGet("preview/constancia-cita/{citaId:guid}")]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(ComprobanteCitaPreviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComprobanteCitaPreviewDto>> PreviewConstanciaCita(Guid citaId)
    {
        var resultado = await _comprobanteService.PreviewConstanciaCitaAsync(citaId);
        return Ok(resultado);
    }

    /// <summary>
    /// Genera una vista previa del resumen de atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite visualizar el contenido del resumen antes de emitirlo.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteVer"/>.
    /// </remarks>
    [HttpGet("preview/resumen-atencion/{atencionId:guid}")]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(ComprobanteAtencionPreviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComprobanteAtencionPreviewDto>> PreviewResumenAtencion(Guid atencionId)
    {
        var resultado = await _comprobanteService.PreviewResumenAtencionAsync(atencionId);
        return Ok(resultado);
    }

    /// <summary>
    /// Genera una vista previa del estado de cuenta de un paciente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite visualizar el contenido del estado de cuenta antes de emitirlo.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteVer"/>.
    /// </remarks>
    [HttpGet("preview/estado-cuenta/paciente/{pacienteId:guid}")]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(ComprobanteEstadoCuentaPreviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComprobanteEstadoCuentaPreviewDto>> PreviewEstadoCuentaPaciente(Guid pacienteId)
    {
        var resultado = await _comprobanteService.PreviewEstadoCuentaPacienteAsync(pacienteId);
        return Ok(resultado);
    }

    // ==========================================================
    // EMISIÓN
    // ==========================================================

    /// <summary>
    /// Emite formalmente una boleta de pago.
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea un comprobante legal de pago y lo registra en el sistema.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteEmitir"/>.
    /// </remarks>
    [HttpPost("emitir/boleta-pago")]
    [Authorize(Policy = PermisosPolicies.ComprobanteEmitir)]
    [Auditoria("Comprobantes", "BoletaPago", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<object>> EmitirBoletaPago([FromBody] EmitirComprobantePagoDto dto)
    {
        var usuarioId = ObtenerUsuarioIdActual();
        if (usuarioId == Guid.Empty) return Unauthorized(new { Mensaje = "Usuario no identificado." });

        var comprobanteId = await _comprobanteService.EmitirBoletaPagoAsync(dto, usuarioId);
        return Ok(new { Mensaje = "Boleta de pago emitida correctamente.", ComprobanteId = comprobanteId });
    }

    /// <summary>
    /// Emite formalmente una constancia de cita.
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea un comprobante que confirma la cita programada.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteEmitir"/>.
    /// </remarks>
    [HttpPost("emitir/constancia-cita")]
    [Authorize(Policy = PermisosPolicies.ComprobanteEmitir)]
    [Auditoria("Comprobantes", "ConstanciaCita", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<object>> EmitirConstanciaCita([FromBody] EmitirComprobanteCitaDto dto)
    {
        var usuarioId = ObtenerUsuarioIdActual();
        if (usuarioId == Guid.Empty) return Unauthorized(new { Mensaje = "Usuario no identificado." });

        var comprobanteId = await _comprobanteService.EmitirConstanciaCitaAsync(dto, usuarioId);
        return Ok(new { Mensaje = "Constancia de cita emitida correctamente.", ComprobanteId = comprobanteId });
    }

    /// <summary>
    /// Emite formalmente un resumen de atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea un comprobante con el resumen de la atención médica.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteEmitir"/>.
    /// </remarks>
    [HttpPost("emitir/resumen-atencion")]
    [Authorize(Policy = PermisosPolicies.ComprobanteEmitir)]
    [Auditoria("Comprobantes", "ResumenAtencion", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<object>> EmitirResumenAtencion([FromBody] EmitirComprobanteAtencionDto dto)
    {
        var usuarioId = ObtenerUsuarioIdActual();
        if (usuarioId == Guid.Empty) return Unauthorized(new { Mensaje = "Usuario no identificado." });

        var comprobanteId = await _comprobanteService.EmitirResumenAtencionAsync(dto, usuarioId);
        return Ok(new { Mensaje = "Resumen de atención emitido correctamente.", ComprobanteId = comprobanteId });
    }

    /// <summary>
    /// Emite formalmente un estado de cuenta de paciente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea un comprobante con el resumen financiero del paciente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteEmitir"/>.
    /// </remarks>
    [HttpPost("emitir/estado-cuenta")]
    [Authorize(Policy = PermisosPolicies.ComprobanteEmitir)]
    [Auditoria("Comprobantes", "EstadoCuenta", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<object>> EmitirEstadoCuenta([FromBody] EmitirComprobanteEstadoCuentaDto dto)
    {
        var usuarioId = ObtenerUsuarioIdActual();
        if (usuarioId == Guid.Empty) return Unauthorized(new { Mensaje = "Usuario no identificado." });

        var comprobanteId = await _comprobanteService.EmitirEstadoCuentaPacienteAsync(dto, usuarioId);
        return Ok(new { Mensaje = "Estado de cuenta emitido correctamente.", ComprobanteId = comprobanteId });
    }

    // ==========================================================
    // PDF
    // ==========================================================

    /// <summary>
    /// Genera y descarga el PDF de una boleta de pago.
    /// </summary>
    /// <remarks>
    /// **Uso:** Descarga el documento PDF de la boleta emitida.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteImprimir"/>.
    /// </remarks>
    [HttpGet("{comprobanteId:guid}/pdf/boleta-pago")]
    [Authorize(Policy = PermisosPolicies.ComprobanteImprimir)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerarPdfBoletaPago(Guid comprobanteId)
    {
        var documento = await _comprobanteService.GenerarPdfBoletaPagoAsync(comprobanteId);
        return File(documento.Archivo, documento.ContentType, documento.NombreArchivo);
    }

    /// <summary>
    /// Genera y descarga el PDF de una constancia de cita.
    /// </summary>
    /// <remarks>
    /// **Uso:** Descarga el documento PDF de la constancia emitida.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteImprimir"/>.
    /// </remarks>
    [HttpGet("{comprobanteId:guid}/pdf/constancia-cita")]
    [Authorize(Policy = PermisosPolicies.ComprobanteImprimir)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerarPdfConstanciaCita(Guid comprobanteId)
    {
        var documento = await _comprobanteService.GenerarPdfConstanciaCitaAsync(comprobanteId);
        return File(documento.Archivo, documento.ContentType, documento.NombreArchivo);
    }

    /// <summary>
    /// Genera y descarga el PDF de un resumen de atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Descarga el documento PDF del resumen emitido.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteImprimir"/>.
    /// </remarks>
    [HttpGet("{comprobanteId:guid}/pdf/resumen-atencion")]
    [Authorize(Policy = PermisosPolicies.ComprobanteImprimir)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerarPdfResumenAtencion(Guid comprobanteId)
    {
        var documento = await _comprobanteService.GenerarPdfResumenAtencionAsync(comprobanteId);
        return File(documento.Archivo, documento.ContentType, documento.NombreArchivo);
    }

    /// <summary>
    /// Genera y descarga el PDF de un estado de cuenta.
    /// </summary>
    /// <remarks>
    /// **Uso:** Descarga el documento PDF del estado de cuenta emitido.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteImprimir"/>.
    /// </remarks>
    [HttpGet("{comprobanteId:guid}/pdf/estado-cuenta")]
    [Authorize(Policy = PermisosPolicies.ComprobanteImprimir)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GenerarPdfEstadoCuenta(Guid comprobanteId)
    {
        var documento = await _comprobanteService.GenerarPdfEstadoCuentaPacienteAsync(comprobanteId);
        return File(documento.Archivo, documento.ContentType, documento.NombreArchivo);
    }

    // ==========================================================
    // CONSULTAS
    // ==========================================================

    /// <summary>
    /// Obtiene los datos de un comprobante por su ID.
    /// </summary>
    /// <remarks>
    /// **Uso:** Recupera la información completa de un comprobante.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteVer"/>.
    /// </remarks>
    [HttpGet("{id:guid}")]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(ComprobanteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ComprobanteDto>> ObtenerPorId(Guid id)
    {
        var resultado = await _comprobanteService.ObtenerPorIdAsync(id);
        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene todos los comprobantes de un paciente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el historial de comprobantes de un paciente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteVer"/>.
    /// </remarks>
    [HttpGet("paciente/{pacienteId:guid}")]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(IEnumerable<ComprobanteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ComprobanteDto>>> ObtenerPorPaciente(Guid pacienteId)
    {
        var resultado = await _comprobanteService.ObtenerPorPacienteAsync(pacienteId);
        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene todos los comprobantes asociados a un pago.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar los comprobantes emitidos para un pago específico.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteVer"/>.
    /// </remarks>
    [HttpGet("pago/{pagoId:guid}")]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(IEnumerable<ComprobanteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ComprobanteDto>>> ObtenerPorPago(Guid pagoId)
    {
        var resultado = await _comprobanteService.ObtenerPorPagoAsync(pagoId);
        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene todos los comprobantes asociados a una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar los comprobantes emitidos en el contexto de una atención.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteVer"/>.
    /// </remarks>
    [HttpGet("atencion/{atencionId:guid}")]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(IEnumerable<ComprobanteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<ComprobanteDto>>> ObtenerPorAtencion(Guid atencionId)
    {
        var resultado = await _comprobanteService.ObtenerPorAtencionAsync(atencionId);
        return Ok(resultado);
    }

    // ==========================================================
    // ANULACIÓN
    // ==========================================================

    /// <summary>
    /// Anula un comprobante emitido.
    /// </summary>
    /// <remarks>
    /// **Uso:** Cambia el estado del comprobante a "Anulado" (eliminación lógica) y registra el motivo.
    /// **Permiso requerido:** <see cref="PermisosPolicies.ComprobanteAnular"/>.
    /// </remarks>
    [HttpPut("{comprobanteId:guid}/anular")]
    [Authorize(Policy = PermisosPolicies.ComprobanteAnular)]
    [Auditoria("Comprobantes", "Comprobante", TipoAccionAuditoria.Eliminacion, NivelAuditoria.Critico)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<object>> AnularComprobante(
        Guid comprobanteId,
        [FromBody] AnularComprobanteRequest request)
    {
        var usuarioId = ObtenerUsuarioIdActual();
        if (usuarioId == Guid.Empty) return Unauthorized(new { Mensaje = "Usuario no identificado." });

        await _comprobanteService.AnularComprobanteAsync(comprobanteId, request.Motivo, usuarioId);
        return Ok(new { Mensaje = "Comprobante anulado correctamente." });
    }
    
    /// <summary>
    /// Obtiene todos los comprobantes emitidos.
    /// </summary>
    [HttpGet]
    [Authorize(Policy = PermisosPolicies.ComprobanteVer)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ComprobanteDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<ComprobanteDto>>>> ObtenerTodos()
    {
        var resultado = await _comprobanteService.ObtenerTodosAsync();
        return Ok(ApiResponse<IEnumerable<ComprobanteDto>>.Ok(resultado, "Comprobantes obtenidos correctamente."));
    }
    
    // ==========================================================
    // HELPERS INTERNOS DE EXTRACCIÓN DE CONTEXTO
    // ==========================================================
    private Guid ObtenerUsuarioIdActual()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                      User.FindFirst("sub")?.Value ??
                      User.FindFirst("usuarioId")?.Value ??
                      User.FindFirst("UsuarioId")?.Value;

        if (Guid.TryParse(idClaim, out var usuarioId))
        {
            return usuarioId;
        }

        return Guid.Empty;
    }

    /// <summary>
    /// Objeto utilizado para solicitar la anulación de un comprobante.
    /// </summary>
    public class AnularComprobanteRequest
    {
        /// <summary>
        /// Motivo de la anulación del comprobante. (Obligatorio, mínimo 3 caracteres).
        /// </summary>
        /// <example>"Error en el monto registrado"</example>
        public string Motivo { get; set; } = string.Empty;
    }
}