using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Atenciones;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services.ATENCIONES;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers.ATENCIONES;

[ApiController]
[Route("api/atenciones/{atencionId:guid}/psicologia")]
[Tags("Módulos Clínicos (Evolución Psicológica Unificada)")]
public class PsicoFormularioController : ControllerBase
{
    private readonly IPsicoFormularioService _psicoService;

    public PsicoFormularioController(IPsicoFormularioService psicoService)
    {
        _psicoService = psicoService;
    }

    /// <summary>
    /// Guarda el progreso parcial o total del Formulario de Evaluación Psicológica de la sesión.
    /// </summary>
    /// <remarks>
    /// **Uso:** El psicólogo puede dar clic en "Guardar Progreso" múltiples veces por pestaña en Blazor. 
    /// Este endpoint aplica lógica Upsert sobre las tablas asociadas sin alterar el estado de caja ni el flujo administrativo central.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.AtencionRegistrar)]
    [Auditoria("AtencionesPsicologia", "FormularioPsicologico", Enums.TipoAccionAuditoria.Edicion, Enums.NivelAuditoria.Importante)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GuardarProgresoClinico(Guid atencionId, [FromBody] RegistrarAtencionDto dto)
    {
        await _psicoService.ActualizarFormularioPsicologicoAsync(atencionId, dto);
        return Ok(ApiResponse<object>.Ok(new { AtencionId = atencionId }, "Secciones del formulario psicológico actualizadas y guardadas correctamente."));
    }
}