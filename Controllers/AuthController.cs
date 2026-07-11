using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.DTOs.Auth;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services;
using psicomedixMonolito.Services.imp;
using psicomedixMonolito.Utils.Filters;

namespace psicomedixMonolito.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Autenticación y Seguridad")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Inicia sesión de un usuario en el sistema.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite a un usuario autenticarse utilizando su correo o nombre de usuario y contraseña.
    /// Retorna un token JWT que debe ser incluido en las siguientes peticiones.
    /// **Permiso:** Público (no requiere autenticación previa).
    /// </remarks>
    [AllowAnonymous]
    [Auditoria("Seguridad", "Usuario", TipoAccionAuditoria.Login, NivelAuditoria.Critico)]
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<RespuestaInicioSesionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Login([FromBody] IniciarSesionDto dto)
    {
        var respuesta = await _authService.IniciarSesionAsync(dto);
        return Ok(ApiResponse<object>.Ok(respuesta, "Inicio de sesión correcto."));
    }

    /// <summary>
    /// Cambia la contraseña del usuario autenticado.
    /// </summary>
    /// <remarks>
    /// **Uso:** Permite al usuario cambiar su propia contraseña. Requiere la contraseña actual para validación.
    /// **Requisito:** Usuario autenticado (cualquier rol).
    /// </remarks>
    [Authorize]
    [Auditoria("Seguridad", "Usuario", TipoAccionAuditoria.Edicion, NivelAuditoria.Critico)]
    [HttpPost("cambiar-contrasena")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CambiarContrasena([FromBody] CambiarContrasenaDto dto)
    {
        // 🚀 Ajuste Monolítico: Extraemos el ID directamente desde los Claims del Token JWT
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                      User.FindFirst("sub")?.Value ??
                      User.FindFirst("usuarioId")?.Value ??
                      User.FindFirst("UsuarioId")?.Value;

        if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out var usuarioId))
        {
            return Unauthorized(new { mensaje = "No se pudo identificar al usuario autenticado en la sesión activa." });
        }

        // Se invoca el método enviando el DTO y el Guid del usuario resuelto
        await _authService.CambiarContrasenaAsync(dto, usuarioId);
        
        return Ok(ApiResponse<object>.Ok(null, "Contraseña actualizada correctamente."));
    }
}