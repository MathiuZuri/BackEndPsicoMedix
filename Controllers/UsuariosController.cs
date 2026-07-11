using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Usuarios;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Authorization;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

/// <summary>
/// Controlador para la gestión de usuarios del sistema.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Seguridad - Usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Obtiene la lista de todos los usuarios del sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Consulta el catálogo completo de usuarios, incluyendo datos básicos.
    /// Útil para la administración de personal y la asignación de roles.
    /// **Permiso requerido:** <see cref="PermisosPolicies.UsuarioVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.UsuarioVer)]
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioService.ObtenerTodosAsync();
        return Ok(ApiResponse<object>.Ok(usuarios, "Usuarios obtenidos correctamente."));
    }

    /// <summary>
    /// Obtiene los datos de un usuario específico por su ID.
    /// </summary>
    /// <remarks>
    /// **Uso:** Recupera la información completa de un usuario individual.
    /// Útil para la pantalla de detalle o para validar datos antes de una actualización.
    /// **Permiso requerido:** <see cref="PermisosPolicies.UsuarioVer"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.UsuarioVer)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UsuarioResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var usuario = await _usuarioService.ObtenerPorIdAsync(id);
        if (usuario == null)
            throw new KeyNotFoundException("Usuario no encontrado.");
        return Ok(ApiResponse<object>.Ok(usuario, "Usuario obtenido correctamente."));
    }

    /// <summary>
    /// Crea un nuevo usuario en el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Registra un nuevo usuario con correo y contraseña. No tiene roles asignados por defecto.
    /// **Permiso requerido:** <see cref="PermisosPolicies.UsuarioCrear"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.UsuarioCrear)]
    [Auditoria("Usuarios", "Usuario", TipoAccionAuditoria.Creacion, NivelAuditoria.Critico)]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CrearUsuarioDto dto)
    {
        var id = await _usuarioService.CrearAsync(dto);
        return CreatedAtAction(
            nameof(GetById),
            new { id },
            ApiResponse<object>.Ok(new { id }, "Usuario creado correctamente.", 201)
        );
    }

    /// <summary>
    /// Actualiza los datos de un usuario existente.
    /// </summary>
    /// <remarks>
    /// **Uso:** Modifica los campos permitidos (nombres, apellidos, nombre de usuario, correo).
    /// **Permiso requerido:** <see cref="PermisosPolicies.UsuarioEditar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.UsuarioEditar)]
    [Auditoria("Usuarios", "Usuario", TipoAccionAuditoria.Edicion, NivelAuditoria.Critico)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] EditarUsuarioDto dto)
    {
        await _usuarioService.ActualizarAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Usuario actualizado correctamente."));
    }

    /// <summary>
    /// Asigna un rol a un usuario específico.
    /// </summary>
    /// <remarks>
    /// **Uso:** Otorga un rol a un usuario. Si ya lo tiene, lanza error.
    /// **Permiso requerido:** <see cref="PermisosPolicies.UsuarioAsignarRol"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.UsuarioAsignarRol)]
    [Auditoria("Usuarios", "UsuarioRol", TipoAccionAuditoria.Asignacion, NivelAuditoria.Critico)]
    [HttpPost("asignar-rol")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRole([FromBody] AsignarRolUsuarioDto dto)
    {
        await _usuarioService.AsignarRolAsync(dto);
        return Ok(ApiResponse<object>.Ok(dto, "Rol asignado correctamente."));
    }

    /// <summary>
    /// Cambia el estado de un usuario.
    /// </summary>
    /// <remarks>
    /// **Uso:** Activa, desactiva o bloquea un usuario. No se puede desactivar al administrador principal.
    /// **Permiso requerido:** <see cref="PermisosPolicies.UsuarioEditar"/>.
    /// </remarks>
    [Authorize(Policy = PermisosPolicies.UsuarioEditar)]
    [Auditoria("Usuarios", "Usuario", TipoAccionAuditoria.Edicion, NivelAuditoria.Critico)]
    [HttpPut("{id:guid}/estado")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] CambiarEstadoUsuarioDto dto)
    {
        await _usuarioService.CambiarEstadoAsync(id, dto);
        return Ok(ApiResponse<object>.Ok(new { id }, "Estado del usuario actualizado correctamente."));
    }
}