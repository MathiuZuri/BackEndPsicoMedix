using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Horarios;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión de horarios y disponibilidad de los doctores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Horarios y Disponibilidad")]
public class HorariosController : ControllerBase
{
    private readonly IHorarioDoctorService _horarioService;

    public HorariosController(IHorarioDoctorService horarioService)
    {
        _horarioService = horarioService;
    }

    /// <summary>
    /// Obtiene todos los horarios registrados en el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Devuelve la lista completa de todos los horarios definidos, independientemente del doctor.
    /// Útil para la administración general de disponibilidad.
    /// **Permiso requerido:** <see cref="PermisosPolicies.HorarioVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.HorarioVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HorarioDoctorResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var horarios = await _horarioService.ObtenerTodosAsync();
        return Ok(ApiResponse<object>.Ok(horarios, "Horarios obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene todos los horarios asociados a un doctor específico.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite consultar la disponibilidad completa de un doctor, incluyendo bloques de tiempo y vigencias.
    /// Útil para la pantalla de gestión de horarios del doctor.
    /// **Permiso requerido:** <see cref="PermisosPolicies.HorarioVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.HorarioVer)]
    [HttpGet("doctor/{doctorId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HorarioDoctorResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByDoctor(Guid doctorId)
    {
        var horarios = await _horarioService.ObtenerPorDoctorAsync(doctorId);
        return Ok(ApiResponse<object>.Ok(horarios, "Horarios del doctor obtenidos correctamente."));
    }

    /// <summary>
    /// Crea un nuevo horario para un doctor.
    /// </summary>
    /// <remarks>
    /// **Uso:** Registra un nuevo bloque horario de disponibilidad para un doctor.
    /// **Permiso requerido:** <see cref="PermisosPolicies.HorarioCrear"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.HorarioCrear)]
    [Auditoria("Horarios", "HorarioDoctor", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CrearHorarioDoctorDto dto)
    {
        var id = await _horarioService.CrearAsync(dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Horario registrado correctamente."));
    }

    /// <summary>
    /// Actualiza un horario existente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Modifica los datos de un horario (día, hora, vigencia, etc.).
    /// **Permiso requerido:** <see cref="PermisosPolicies.HorarioEditar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.HorarioEditar)]
    [Auditoria("Horarios", "HorarioDoctor", TipoAccionAuditoria.Edicion, NivelAuditoria.Importante)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] EditarHorarioDoctorDto dto)
    {
        await _horarioService.ActualizarAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Horario actualizado correctamente."));
    }

    /// <summary>
    /// Obtiene la matriz semanal de disponibilidad de un doctor.
    /// </summary>
    /// <remarks>
    /// **Uso:** Proporciona una representación visual (matriz) de la disponibilidad
    /// de un doctor para una semana completa, con bloques de 30 minutos.
    /// **Permiso requerido:** <see cref="PermisosPolicies.HorarioVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.HorarioVer)]
    [HttpGet("doctor/{doctorId:guid}/matriz")]
    [ProducesResponseType(typeof(ApiResponse<MatrizSemanalDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMatrizSemanal(Guid doctorId, [FromQuery] string? fecha)
    {
        var fechaFiltro = string.IsNullOrEmpty(fecha) 
            ? DateOnly.FromDateTime(DateTime.Today) 
            : DateOnly.Parse(fecha);
        var matriz = await _horarioService.ObtenerMatrizSemanalAsync(doctorId, fechaFiltro);
        return Ok(ApiResponse<object>.Ok(matriz, "Matriz semanal calculada correctamente."));
    }
}