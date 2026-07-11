using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Auth;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Utils.Helpers;

// Tu DbContext unificado
// Tu utilitario JwtHelper

namespace psicomedixMonolito.Services.imp;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;

    // Dependencia exclusiva del contexto de datos y el utilitario de criptografía de tokens
    public AuthService(
        ApplicationDbContext context,
        JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }

    // ==========================================================
    // INICIO DE SESIÓN Y CARGA DE ÁRBOL DE SEGURIDAD
    // ==========================================================
    public async Task<RespuestaInicioSesionDto> IniciarSesionAsync(IniciarSesionDto dto)
    {
        // Consulta unificada profunda: Trae al usuario y mapea todo su árbol de permisos en un solo viaje
        var usuario = await _context.Set<Usuario>()
            .Include(x => x.UsuarioRoles)
                .ThenInclude(x => x.Rol)
                .ThenInclude(x => x.RolPermisos)
                .ThenInclude(x => x.Permiso)
            .FirstOrDefaultAsync(x => x.Correo == dto.UsuarioOCorreo.Trim() || 
                                      x.UserName == dto.UsuarioOCorreo.Trim());

        if (usuario == null)
            throw new InvalidOperationException("Usuario o contraseña incorrectos.");
        
        if (usuario.Estado != EstadoUsuario.Activo)
            throw new InvalidOperationException("Tu cuenta no está activa. Contacta al administrador.");

        // Verificación de Hash nativa
        var passwordValido = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);

        if (!passwordValido)
            throw new InvalidOperationException("Usuario o contraseña incorrectos.");

        // Extracción y aplanado de roles activos
        var roles = usuario.UsuarioRoles
            .Where(x => x.Activo)
            .Select(x => x.Rol.Nombre)
            .Distinct()
            .ToList();

        // Extracción y aplanado de códigos de permisos activos
        var permisos = usuario.UsuarioRoles
            .Where(x => x.Activo)
            .SelectMany(x => x.Rol.RolPermisos)
            .Select(x => x.Permiso.Codigo)
            .Distinct()
            .ToList();

        // Delegación de firma criptográfica
        var token = _jwtHelper.GenerarToken(usuario, roles, permisos);

        return new RespuestaInicioSesionDto
        {
            UsuarioId = usuario.Id,
            CodigoUsuario = usuario.CodigoUsuario,
            NombreCompleto = $"{usuario.Nombres} {usuario.Apellidos}",
            Correo = usuario.Correo,
            Token = token,
            Roles = roles,
            Permisos = permisos,
            DebeCambiarContrasena = usuario.DebeCambiarContrasena || usuario.UltimoAcceso == null
        };
    }
    
    // ==========================================================
    // CAMBIO DE CREDENCIALES (DESACOPLADO DE INFRAESTRUCTURA)
    // ==========================================================
    public async Task CambiarContrasenaAsync(CambiarContrasenaDto dto, Guid usuarioId)
    {
        var usuario = await _context.Set<Usuario>().FindAsync(usuarioId)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        // Verificar la firma de la contraseña actual
        if (!BCrypt.Net.BCrypt.Verify(dto.ContrasenaActual, usuario.PasswordHash))
            throw new InvalidOperationException("La contraseña actual es incorrecta.");

        // Hashear y actualizar el estado de vigencia de la cuenta
        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.ContrasenaNueva);
        usuario.DebeCambiarContrasena = false;
        usuario.UltimoAcceso = DateTime.UtcNow;

        // Confirmación directa en la base de datos de Psicomedix
        await _context.SaveChangesAsync();
    }
}