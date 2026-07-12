using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Pacientes;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models; // Ajustado al namespace estándar de tus filtros de auditoría
using psicomedixMonolito.Services;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión completa de pacientes del sistema.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Gestión de Pacientes")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    /// <summary>
    /// Obtiene la lista de todos los pacientes registrados.
    /// </summary>
    /// <remarks>
    /// **Uso:** Devuelve el catálogo completo de pacientes, independientemente de su estado.
    /// Útil para administración y búsqueda general.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PacienteVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PacienteVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PacienteResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var pacientes = await _pacienteService.ObtenerTodosAsync();
        return Ok(ApiResponse<object>.Ok(pacientes, "Pacientes obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene los datos de un paciente por su ID.
    /// </summary>
    /// <remarks>
    /// **Uso:** Recupera la información completa de un paciente, incluyendo su historial clínico.
    /// Útil para la ficha del paciente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PacienteVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PacienteVer)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PacienteResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var paciente = await _pacienteService.ObtenerPorIdAsync(id);
        if (paciente == null)
            throw new KeyNotFoundException("Paciente no encontrado.");
            
        return Ok(ApiResponse<object>.Ok(paciente, "Paciente obtenido correctamente."));
    }

    /// <summary>
    /// Obtiene los datos de un paciente por su número de DNI.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite buscar un paciente rápidamente por DNI.
    /// Útil para la admisión y recepción.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PacienteVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PacienteVer)]
    [HttpGet("dni/{dni}")]
    [ProducesResponseType(typeof(ApiResponse<PacienteResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDni(string dni)
    {
        var paciente = await _pacienteService.ObtenerPorDniAsync(dni);
        if (paciente == null)
            throw new KeyNotFoundException("Paciente no encontrado.");
            
        return Ok(ApiResponse<object>.Ok(paciente, "Paciente obtenido correctamente."));
    }

    /// <summary>
    /// Registra un nuevo paciente en el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Crea un nuevo paciente y apertura su historial clínico automáticamente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PacienteCrear"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PacienteCrear)]
    [Auditoria("Pacientes", "Paciente", TipoAccionAuditoria.Creacion, NivelAuditoria.Importante)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CrearPacienteDto dto)
    {
        // Extraemos el ID del usuario actual desde el Token JWT
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                      User.FindFirst("sub")?.Value ??
                      User.FindFirst("usuarioId")?.Value ??
                      User.FindFirst("UsuarioId")?.Value;

        if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out var usuarioId))
        {
            return Unauthorized(new { mensaje = "No se pudo identificar al usuario autenticado en la sesión activa." });
        }

        // Se envía el DTO junto con el usuarioId unificado para las trazas de auditoría e historial clínico
        var id = await _pacienteService.CrearAsync(dto, usuarioId);
    
        return CreatedAtAction(
            nameof(GetById),
            new { id },
            ApiResponse<object>.Ok(new { id }, "Paciente registrado correctamente.", 201)
        );
    }

    /// <summary>
    /// Actualiza los datos de contacto de un paciente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Modifica el celular, correo o dirección del paciente.
    /// **Permiso requerido:** <see cref="PermisosPolicies.PacienteEditar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PacienteEditar)]
    [Auditoria("Pacientes", "Paciente", TipoAccionAuditoria.Edicion, NivelAuditoria.Importante)]
    [HttpPut("{id:guid}/contacto")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ActualizarContactoPacienteDto dto)
    {
        await _pacienteService.ActualizarContactoAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Contacto del paciente actualizado correctamente."));
    }

    /// <summary>
    /// Cambia el estado de un paciente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite cambiar el estado (Activo, Inactivo, Bloqueado, Fallecido, Eliminado).
    /// **Permiso requerido:** <see cref="PermisosPolicies.PacienteEditar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.PacienteEditar)]
    [Auditoria("Pacientes", "Paciente", TipoAccionAuditoria.Edicion, NivelAuditoria.Importante)]
    [HttpPut("{id:guid}/estado")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] CambiarEstadoPacienteDto dto)
    {
        await _pacienteService.CambiarEstadoAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Estado del paciente actualizado correctamente."));
    }
}