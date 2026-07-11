using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Comunes;
using psicomedixMonolito.DTOs.Finanzas;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión financiera y contable del sistema.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Finanzas y Contabilidad")]
public class FinanzasController : ControllerBase
{
    private readonly IFinanzasService _finanzasService;

    public FinanzasController(IFinanzasService finanzasService)
    {
        _finanzasService = finanzasService;
    }

    // ==========================================================
    // RESÚMENES
    // ==========================================================

    /// <summary>
    /// Obtiene el resumen diario de finanzas (ingresos, pagos, deudas).
    /// </summary>
    /// <remarks>
    /// **Uso:** Proporciona un panorama de la actividad financiera de un día específico.
    /// Útil para el cierre de caja diario y conciliación contable.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("resumen-diario")]
    [ProducesResponseType(typeof(ApiResponse<ResumenDiarioFinanzasDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerResumenDiario([FromQuery] DateOnly fecha)
    {
        var resumen = await _finanzasService.ObtenerResumenDiarioAsync(fecha);
        return Ok(ApiResponse<object>.Ok(resumen, "Resumen diario de finanzas obtenido correctamente."));
    }

    /// <summary>
    /// Obtiene el resumen mensual de finanzas (agrupado por día).
    /// </summary>
    /// <remarks>
    /// **Uso:** Proporciona un desglose financiero completo de un mes específico.
    /// Útil para reportes de gestión mensual y auditoría.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("resumen-mensual")]
    [ProducesResponseType(typeof(ApiResponse<ResumenMensualFinanzasDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerResumenMensual([FromQuery] int anio, [FromQuery] int mes)
    {
        var resumen = await _finanzasService.ObtenerResumenMensualAsync(anio, mes);
        return Ok(ApiResponse<object>.Ok(resumen, "Resumen mensual de finanzas obtenido correctamente."));
    }

    /// <summary>
    /// Obtiene el resumen anual de finanzas (agrupado por mes).
    /// </summary>
    /// <remarks>
    /// **Uso:** Proporciona una visión global de la actividad financiera de todo un año.
    /// Útil para balances anuales y análisis de tendencias.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("resumen-anual")]
    [ProducesResponseType(typeof(ApiResponse<ResumenAnualFinanzasDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerResumenAnual([FromQuery] int anio)
    {
        var resumen = await _finanzasService.ObtenerResumenAnualAsync(anio);
        return Ok(ApiResponse<object>.Ok(resumen, "Resumen anual de finanzas obtenido correctamente."));
    }

    // ==========================================================
    // PAGOS (filtrados por estado)
    // ==========================================================

    /// <summary>
    /// Obtiene todos los pagos pendientes (con saldo pendiente > 0).
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite identificar rápidamente los pagos no liquidados completamente.
    /// Útil para el seguimiento de cuentas por cobrar.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("pagos-pendientes")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PagoFinanzasDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerPagosPendientes()
    {
        var pagos = await _finanzasService.ObtenerPagosPendientesAsync();
        return Ok(ApiResponse<object>.Ok(pagos, "Pagos pendientes obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene todos los pagos completamente pagados (saldo pendiente = 0).
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el historial de pagos completados.
    /// Útil para reportes de ingresos y conciliación bancaria.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("pagos-pagados")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PagoFinanzasDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerPagosPagados()
    {
        var pagos = await _finanzasService.ObtenerPagosPagadosAsync();
        return Ok(ApiResponse<object>.Ok(pagos, "Pagos pagados obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene todos los pagos parciales (estado "Parcial").
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite identificar los pagos abonados parcialmente que aún tienen saldo pendiente.
    /// Útil para el seguimiento de deudas fraccionadas.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("pagos-parciales")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PagoFinanzasDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerPagosParciales()
    {
        var pagos = await _finanzasService.ObtenerPagosParcialesAsync();
        return Ok(ApiResponse<object>.Ok(pagos, "Pagos parciales obtenidos correctamente."));
    }

    // ==========================================================
    // CONSULTAS ESPECÍFICAS
    // ==========================================================

    /// <summary>
    /// Obtiene un pago específico por su código de pago.
    /// </summary>
    /// <remarks>
    /// **Uso:** Busca un pago de forma rápida utilizando el código único generado por el sistema.
    /// Útil para validaciones en caja o para la emisión de comprobantes.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("pago/codigo/{codigoPago}")]
    [ProducesResponseType(typeof(ApiResponse<PagoFinanzasDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPagoPorCodigo(string codigoPago)
    {
        var pago = await _finanzasService.ObtenerPagoPorCodigoAsync(codigoPago);
        if (pago == null)
            throw new KeyNotFoundException("Pago no encontrado.");
        return Ok(ApiResponse<object>.Ok(pago, "Pago obtenido correctamente."));
    }

    /// <summary>
    /// Obtiene el estado de cuenta completo de un paciente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Proporciona un resumen financiero completo de un paciente (total facturado, pagado y pendiente).
    /// Útil para informar al paciente o para auditoría.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("paciente/{pacienteId:guid}/estado-cuenta")]
    [ProducesResponseType(typeof(ApiResponse<EstadoCuentaPacienteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerEstadoCuentaPaciente(Guid pacienteId)
    {
        var estadoCuenta = await _finanzasService.ObtenerEstadoCuentaPacienteAsync(pacienteId);
        return Ok(ApiResponse<object>.Ok(estadoCuenta, "Estado de cuenta del paciente obtenido correctamente."));
    }

    /// <summary>
    /// Obtiene la lista de deudas reales (atenciones con saldo pendiente) de todo el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Identifica globalmente todas las atenciones que aún tienen saldo pendiente.
    /// Útil para la gestión de cobranza y análisis de cartera.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("deudas-reales")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EstadoPagoAtencionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerDeudasReales()
    {
        var deudas = await _finanzasService.ObtenerDeudasRealesAsync();
        return Ok(ApiResponse<object>.Ok(deudas, "Deudas reales obtenidas correctamente."));
    }

    /// <summary>
    /// Obtiene la lista de deudas reales de un paciente específico.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar las deudas pendientes de un paciente en particular.
    /// Útil para el área de cobranza o para informar al paciente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("paciente/{pacienteId:guid}/deudas-reales")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EstadoPagoAtencionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerDeudasRealesPaciente(Guid pacienteId)
    {
        var deudas = await _finanzasService.ObtenerDeudasRealesPacienteAsync(pacienteId);
        return Ok(ApiResponse<object>.Ok(deudas, "Deudas reales del paciente obtenidas correctamente."));
    }

    /// <summary>
    /// Obtiene el estado de pago de una atención médica específica.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite conocer el estado financiero de una atención (pagada, parcial, pendiente, sobrepagada).
    /// Útil para el área de caja al registrar nuevos pagos.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("atencion/{atencionId:guid}/estado-pago")]
    [ProducesResponseType(typeof(ApiResponse<EstadoPagoAtencionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerEstadoPagoAtencion(Guid atencionId)
    {
        var estadoPago = await _finanzasService.ObtenerEstadoPagoAtencionAsync(atencionId);
        return Ok(ApiResponse<object>.Ok(estadoPago, "Estado de pago de la atención obtenido correctamente."));
    }

    // ==========================================================
    // REPORTES AVANZADOS
    // ==========================================================

    /// <summary>
    /// Obtiene el libro diario de finanzas para una fecha específica.
    /// </summary>
    /// <remarks>
    /// **Uso:** Proporciona un registro cronológico de todos los pagos realizados en una fecha.
    /// Útil para la contabilidad y auditoría financiera.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("libro-diario")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PagoFinanzasDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerLibroDiario([FromQuery] DateOnly fecha)
    {
        var resultado = await _finanzasService.ObtenerLibroDiarioAsync(fecha);
        return Ok(ApiResponse<object>.Ok(resultado, "Libro diario obtenido correctamente."));
    }

    /// <summary>
    /// Obtiene un resumen financiero mensual completo (caja, atenciones, ajustes).
    /// </summary>
    /// <remarks>
    /// **Uso:** Proporciona la vista más completa de la actividad financiera de un mes.
    /// Útil para el cierre contable mensual y la toma de decisiones gerenciales.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("resumen-financiero-mensual-completo")]
    [ProducesResponseType(typeof(ApiResponse<ResumenFinancieroMensualCompletoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerResumenFinancieroMensualCompleto(
        [FromQuery] int anio,
        [FromQuery] int mes)
    {
        var resultado = await _finanzasService.ObtenerResumenFinancieroMensualCompletoAsync(anio, mes);
        return Ok(ApiResponse<object>.Ok(resultado, "Resumen financiero mensual completo obtenido correctamente."));
    }

    // ==========================================================
    // AJUSTES FINANCIEROS
    // ==========================================================

    /// <summary>
    /// Registra un nuevo ajuste financiero (descuento, recargo, corrección, etc.).
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea un registro de ajuste financiero asociado a un pago para corregir montos o aplicar descuentos.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PagoRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PagoRegistrar)]
    [Auditoria("Finanzas", "Ajuste financiero", TipoAccionAuditoria.Creacion, NivelAuditoria.Critico)]
    [HttpPost("ajustes-financieros")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegistrarAjusteFinanciero([FromBody] RegistrarAjusteFinancieroDto dto)
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

        // Se envía el DTO junto con el usuarioId resuelto al método unificado
        var id = await _finanzasService.RegistrarAjusteFinancieroAsync(dto, usuarioId);
    
        return Ok(ApiResponse<object>.Ok(new { id }, "Ajuste financiero registrado correctamente."));
    }

    /// <summary>
    /// Obtiene todos los ajustes financieros registrados en el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el historial completo de ajustes financieros.
    /// Útil para auditoría y seguimiento de correcciones contables.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("ajustes-financieros")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AjusteFinancieroDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerAjustesFinancieros()
    {
        var resultado = await _finanzasService.ObtenerAjustesFinancierosAsync();
        return Ok(ApiResponse<object>.Ok(resultado, "Ajustes financieros obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene los ajustes financieros asociados a una atención médica específica.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar los ajustes relacionados con una atención en particular.
    /// Útil para analizar el historial financiero de una atención.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("atencion/{atencionId:guid}/ajustes-financieros")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AjusteFinancieroDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerAjustesPorAtencion(Guid atencionId)
    {
        var resultado = await _finanzasService.ObtenerAjustesPorAtencionAsync(atencionId);
        return Ok(ApiResponse<object>.Ok(resultado, "Ajustes financieros de la atención obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene los ajustes financieros asociados a un pago específico.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar los ajustes relacionados con un pago en particular.
    /// Útil para rastrear correcciones contables aplicadas a un pago.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("pago/{pagoId:guid}/ajustes-financieros")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AjusteFinancieroDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerAjustesPorPago(Guid pagoId)
    {
        var resultado = await _finanzasService.ObtenerAjustesPorPagoAsync(pagoId);
        return Ok(ApiResponse<object>.Ok(resultado, "Ajustes financieros del pago obtenidos correctamente."));
    }

    // ==========================================================
    // PAGOS PENDIENTES CON PAGINACIÓN
    // ==========================================================

    /// <summary>
    /// Obtiene los pagos pendientes de forma paginada.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite obtener una lista paginada de pagos con saldo pendiente.
    /// Útil para interfaces de usuario con grandes volúmenes de datos.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("pagos-pendientes-paginado")]
    [ProducesResponseType(typeof(ApiResponse<PaginacionResponseDto<PagoFinanzasDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerPagosPendientesPaginado([FromQuery] PaginacionRequestDto request)
    {
        var resultado = await _finanzasService.ObtenerPagosPendientesPaginadosAsync(request);
        return Ok(ApiResponse<object>.Ok(resultado, "Pagos pendientes paginados obtenidos correctamente."));
    }

    // ==========================================================
    // ESTADO DE PAGO DETALLADO DE UNA ATENCIÓN
    // ==========================================================

    /// <summary>
    /// Obtiene el estado de pago detallado de una atención médica específica.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite conocer en detalle la situación financiera de una atención,
    /// incluyendo montos, saldos y resumen de pagos.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("atencion/{atencionId:guid}/estado-pago-detallado")]
    [ProducesResponseType(typeof(ApiResponse<EstadoPagoAtencionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerEstadoPagoAtencionDetallado(Guid atencionId)
    {
        var estado = await _finanzasService.ObtenerEstadoPagoAtencionDetalladoAsync(atencionId);
        return Ok(ApiResponse<object>.Ok(estado, "Estado de pago detallado obtenido correctamente."));
    }

    /// <summary>
    /// Obtiene la tasa actual del Impuesto General a las Ventas (IGV).
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite a los clientes de la API conocer la tasa de impuesto configurada en el sistema.
    /// **Permiso requerido:** <see cref="PermisosPolicies.FinanzasVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.FinanzasVer)]
    [HttpGet("tasa-igv")]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult ObtenerTasaIgv()
    {
        var tasa = (decimal)TasaImpuesto.IGV_18;
        return Ok(ApiResponse<object>.Ok(tasa, "Tasa de IGV actual."));
    }
}