using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services.imp.ATENCIONES;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión evolutiva de los módulos clínicos de una Atención.
/// </summary>
/// <remarks>
/// **Enrutamiento de Sub-recursos:**
/// Todos los endpoints aquí asumen que la Atención Core ya fue creada y utilizan la ruta `/api/atenciones/{atencionId}/[modulo]`.
/// Esto permite que el médico añada múltiples exámenes físicos, ecografías o tactos vaginales a lo largo de las horas que dura la atención.
/// </remarks>
[ApiController]
[Route("api/atenciones/{atencionId:guid}")]
[Tags("Módulos Clínicos (Evolución)")]
public class AtencionesObstetricasController : ControllerBase
{
    private readonly IAnamnesisService _anamnesisService;
    private readonly IExamenFisicoService _examenFisicoService;
    private readonly ITactoVaginalService _tactoVaginalService;
    private readonly IEcografiaObstetricaService _ecografiaService;
    private readonly IImpresionDiagnosticaService _diagnosticoService;

    public AtencionesObstetricasController(
        IAnamnesisService anamnesisService,
        IExamenFisicoService examenFisicoService,
        ITactoVaginalService tactoVaginalService,
        IEcografiaObstetricaService ecografiaService,
        IImpresionDiagnosticaService diagnosticoService)
    {
        _anamnesisService = anamnesisService;
        _examenFisicoService = examenFisicoService;
        _tactoVaginalService = tactoVaginalService;
        _ecografiaService = ecografiaService;
        _diagnosticoService = diagnosticoService;
    }

    // ==========================================
    // 1. ANAMNESIS
    // ==========================================

    /// <summary>
    /// Obtiene la anamnesis asociada a una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar los datos de anamnesis (motivo de consulta, antecedentes, etc.) de una atención específica.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionVer)]
    [HttpGet("anamnesis")]
    [ProducesResponseType(typeof(ApiResponse<AnamnesisDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerAnamnesis(Guid atencionId)
    {
        var result = await _anamnesisService.ObtenerPorAtencionAsync(atencionId);
        
        if (result == null) 
            return NotFound(ApiResponse<object>.Error("Anamnesis no encontrada para esta atención.", 404));
            
        return Ok(ApiResponse<object>.Ok(result, "Anamnesis obtenida correctamente."));
    }

    /// <summary>
    /// Registra la anamnesis para una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Guarda los datos de anamnesis (motivo de consulta, antecedentes obstétricos, etc.) para una atención.
    /// Solo se puede registrar una anamnesis por atención.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionRegistrar)]
    [Auditoria("Atenciones", "Anamnesis", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost("anamnesis")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegistrarAnamnesis(Guid atencionId, [FromBody] AnamnesisDto dto)
    {
        var id = await _anamnesisService.RegistrarAsync(atencionId, dto);
        return CreatedAtAction(nameof(ObtenerAnamnesis), new { atencionId }, ApiResponse<object>.Ok(new { Id = id }, "Anamnesis registrada.", 201));
    }

    // ==========================================
    // 2. EXÁMENES FÍSICOS
    // ==========================================

    /// <summary>
    /// Obtiene todos los exámenes físicos de una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el listado de exámenes físicos realizados durante una atención (pueden ser múltiples).
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionVer)]
    [HttpGet("examenes-fisicos")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ExamenFisicoDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerExamenesFisicos(Guid atencionId)
    {
        var result = await _examenFisicoService.ObtenerPorAtencionAsync(atencionId);
        return Ok(ApiResponse<object>.Ok(result, "Exámenes físicos obtenidos correctamente."));
    }

    /// <summary>
    /// Registra un nuevo examen físico para una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Guarda los datos de un examen físico (funciones vitales, signos obstétricos, etc.) para una atención.
    /// Se pueden registrar múltiples exámenes a lo largo de la atención.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionRegistrar)]
    [Auditoria("Atenciones", "ExamenFisico", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost("examenes-fisicos")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegistrarExamenFisico(Guid atencionId, [FromBody] ExamenFisicoDto dto)
    {
        var id = await _examenFisicoService.RegistrarAsync(atencionId, dto);
        return Ok(ApiResponse<object>.Ok(new { Id = id }, "Examen físico agregado a la atención correctamente."));
    }

    // ==========================================
    // 3. TACTOS VAGINALES
    // ==========================================

    /// <summary>
    /// Obtiene todos los tactos vaginales de una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el listado de tactos vaginales realizados durante una atención (partograma).
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionVer)]
    [HttpGet("tactos-vaginales")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TactoVaginalDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerTactosVaginales(Guid atencionId)
    {
        var result = await _tactoVaginalService.ObtenerPorAtencionAsync(atencionId);
        return Ok(ApiResponse<object>.Ok(result, "Tactos vaginales obtenidos correctamente."));
    }

    /// <summary>
    /// Registra un nuevo tacto vaginal para una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Guarda los datos de un tacto vaginal (dilatación, borramiento, altura de presentación, etc.) para una atención.
    /// Se pueden registrar múltiples tactos para el seguimiento del parto.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionRegistrar)]
    [Auditoria("Atenciones", "TactoVaginal", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost("tactos-vaginales")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegistrarTactoVaginal(Guid atencionId, [FromBody] TactoVaginalDto dto)
    {
        var id = await _tactoVaginalService.RegistrarAsync(atencionId, dto);
        return Ok(ApiResponse<object>.Ok(new { Id = id }, "Tacto vaginal agregado a la atención correctamente."));
    }

    // ==========================================
    // 4. ECOGRAFÍAS OBSTÉTRICAS
    // ==========================================

    /// <summary>
    /// Obtiene todas las ecografías obstétricas de una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el listado de ecografías realizadas durante una atención.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionVer)]
    [HttpGet("ecografias")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EcografiaObstetricaDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerEcografias(Guid atencionId)
    {
        var result = await _ecografiaService.ObtenerPorAtencionAsync(atencionId);
        return Ok(ApiResponse<object>.Ok(result, "Ecografías obtenidas correctamente."));
    }

    /// <summary>
    /// Registra una nueva ecografía obstétrica para una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Guarda los datos de una ecografía (medidas fetales, líquido amniótico, placenta, etc.) para una atención.
    /// Se pueden registrar múltiples ecografías durante la atención.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionRegistrar)]
    [Auditoria("Atenciones", "EcografiaObstetrica", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost("ecografias")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegistrarEcografia(Guid atencionId, [FromBody] EcografiaObstetricaDto dto)
    {
        var id = await _ecografiaService.RegistrarAsync(atencionId, dto);
        return Ok(ApiResponse<object>.Ok(new { Id = id }, "Ecografía agregada a la atención correctamente."));
    }

    // ==========================================
    // 5. IMPRESIÓN DIAGNÓSTICA
    // ==========================================

    /// <summary>
    /// Obtiene la impresión diagnóstica de una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el diagnóstico principal, diagnósticos secundarios, indicaciones y fecha de próxima cita de una atención.
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionVer)]
    [HttpGet("diagnostico")]
    [ProducesResponseType(typeof(ApiResponse<ImpresionDiagnosticaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ObtenerDiagnostico(Guid atencionId)
    {
        var result = await _diagnosticoService.ObtenerPorAtencionAsync(atencionId);
        
        if (result == null) 
            return NotFound(ApiResponse<object>.Error("Aún no hay un diagnóstico para esta atención.", 404));
            
        return Ok(ApiResponse<object>.Ok(result, "Diagnóstico obtenido correctamente."));
    }

    /// <summary>
    /// Registra o actualiza la impresión diagnóstica de una atención.
    /// </summary>
    /// <remarks>
    /// **Uso:** Guarda el diagnóstico principal, diagnósticos secundarios, indicaciones y fecha de próxima cita para una atención.
    /// Solo se puede registrar un diagnóstico por atención (si ya existe, se actualiza).
    /// **Permiso requerido:** <see cref="PermisosPolicies.AtencionRegistrar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionRegistrar)]
    [Auditoria("Atenciones", "ImpresionDiagnostica", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost("diagnostico")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegistrarDiagnostico(Guid atencionId, [FromBody] ImpresionDiagnosticaDto dto)
    {
        var id = await _diagnosticoService.RegistrarAsync(atencionId, dto);
        return CreatedAtAction(nameof(ObtenerDiagnostico), new { atencionId }, ApiResponse<object>.Ok(new { Id = id }, "Diagnóstico registrado.", 201));
    }
}