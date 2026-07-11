using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Citas;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión integral de citas médicas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Gestión de Citas")]
public class CitasController : ControllerBase
{
    private readonly ICitaService _citaService;

    public CitasController(ICitaService citaService)
    {
        _citaService = citaService;
    }

    /// <summary>
    /// Obtiene todas las citas registradas en el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Devuelve el listado completo de todas las citas, incluyendo información del paciente, doctor, servicio y estado.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CitaVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.CitaVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CitaResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var citas = await _citaService.ObtenerTodasAsync();
        return Ok(ApiResponse<object>.Ok(citas, "Citas obtenidas correctamente."));
    }

    /// <summary>
    /// Obtiene los detalles de una cita específica por su ID.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite recuperar la información completa de una cita individual.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CitaVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.CitaVer)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CitaResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var cita = await _citaService.ObtenerPorIdAsync(id);
        if (cita == null)
            throw new KeyNotFoundException("Cita no encontrada.");
        return Ok(ApiResponse<object>.Ok(cita, "Cita obtenida correctamente."));
    }

    /// <summary>
    /// Obtiene todas las citas asociadas a un paciente específico.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar el historial completo de citas de un paciente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CitaVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.CitaVer)]
    [HttpGet("paciente/{pacienteId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CitaResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByPaciente(Guid pacienteId)
    {
        var citas = await _citaService.ObtenerPorPacienteAsync(pacienteId);
        return Ok(ApiResponse<IEnumerable<CitaResponseDto>>.Ok(citas, "Citas del paciente obtenidas correctamente."));
    }

    /// <summary>
    /// Obtiene todas las citas asociadas a un doctor específico.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar la agenda completa de un doctor.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CitaVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.CitaVer)]
    [HttpGet("doctor/{doctorId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CitaResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByDoctor(Guid doctorId)
    {
        var citas = await _citaService.ObtenerPorDoctorAsync(doctorId);
        return Ok(ApiResponse<object>.Ok(citas, "Citas del doctor obtenidas correctamente."));
    }

    /// <summary>
    /// Programa una nueva cita.
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea una nueva cita asociando paciente, doctor, servicio y horario.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CitaProgramar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.CitaProgramar)]
    [Auditoria("Citas", "Cita", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CrearCitaDto dto)
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

        // Se envía el DTO junto con el usuarioId resuelto
        var id = await _citaService.CrearAsync(dto, usuarioId);
    
        return CreatedAtAction(
            nameof(GetById),
            new { id },
            ApiResponse<object>.Ok(new { id }, "Cita programada correctamente.", 201)
        );
    }

    /// <summary>
    /// Reprograma una cita existente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite cambiar la fecha, hora o doctor de una cita ya programada.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CitaReprogramar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.CitaReprogramar)]
    [Auditoria("Citas", "Cita", TipoAccionAuditoria.Edicion, NivelAuditoria.Importante)]
    [HttpPut("{id:guid}/reprogramar")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reprogramar(Guid id, [FromBody] ReprogramarCitaDto dto)
    {
        await _citaService.ReprogramarAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Cita reprogramada correctamente."));
    }

    /// <summary>
    /// Cancela una cita existente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Cambia el estado de la cita a "Cancelada" (eliminación lógica) y registra el motivo.
    /// **Permiso requerido:** <see cref="PermisosPolicies.CitaCancelar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.CitaCancelar)]
    [Auditoria("Citas", "Cita", TipoAccionAuditoria.Eliminacion, NivelAuditoria.Critico)]
    [HttpPut("{id:guid}/cancelar")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancelar(Guid id, [FromBody] CancelarCitaDto dto)
    {
        await _citaService.CancelarAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Cita cancelada correctamente."));
    }
}