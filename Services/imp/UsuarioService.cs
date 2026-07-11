using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Usuarios;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;
public class UsuarioService : IUsuarioService
{
    private readonly ApplicationDbContext _context;

    // Inyectamos únicamente el DbContext centralizado de la aplicación
    public UsuarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE
    // ==========================================================

    public async Task<IEnumerable<UsuarioResponseDto>> ObtenerTodosAsync()
    {
        var usuarios = await _context.Set<Usuario>()
            .AsNoTracking()
            .ToListAsync();

        return usuarios.Select(x => new UsuarioResponseDto
        {
            Id = x.Id,
            CodigoUsuario = x.CodigoUsuario,
            Nombres = x.Nombres,
            Apellidos = x.Apellidos,
            UserName = x.UserName,
            Correo = x.Correo,
            Estado = x.Estado,
            FechaRegistro = x.FechaRegistro,
            UltimoAcceso = x.UltimoAcceso
        });
    }

    public async Task<UsuarioResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        var usuario = await _context.Set<Usuario>().FindAsync(id);
        if (usuario == null) return null;

        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            CodigoUsuario = usuario.CodigoUsuario,
            Nombres = usuario.Nombres,
            Apellidos = usuario.Apellidos,
            UserName = usuario.UserName,
            Correo = usuario.Correo,
            Estado = usuario.Estado,
            FechaRegistro = usuario.FechaRegistro,
            UltimoAcceso = usuario.UltimoAcceso
        };
    }

    // ==========================================================
    // MANTENIMIENTO DE CUENTAS Y CONTROL DE ACCESOS
    // ==========================================================

    public async Task<Guid> CrearAsync(CrearUsuarioDto dto)
    {
        // 🚀 MEJORA MONOLÍTICA: Validamos duplicados usando AnyAsync sin traer relaciones anidadas a memoria
        var existeCorreo = await _context.Set<Usuario>().AnyAsync(x => x.Correo.Trim().ToLower() == dto.Correo.Trim().ToLower());
        if (existeCorreo)
            throw new InvalidOperationException("Ya existe un usuario con ese correo.");

        var existeUserName = await _context.Set<Usuario>().AnyAsync(x => x.UserName.Trim().ToLower() == dto.UserName.Trim().ToLower());
        if (existeUserName)
            throw new InvalidOperationException("Ya existe un usuario con ese nombre de usuario.");

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            CodigoUsuario = GenerarCodigoUsuario(),
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            UserName = dto.UserName.Trim(),
            Correo = dto.Correo.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FechaRegistro = DateTime.UtcNow,
            Estado = EstadoUsuario.Activo,
            DebeCambiarContrasena = true
        };

        await _context.Set<Usuario>().AddAsync(usuario);
        await _context.SaveChangesAsync();

        return usuario.Id;
    }

    public async Task ActualizarAsync(Guid id, EditarUsuarioDto dto)
    {
        var usuario = await _context.Set<Usuario>().FindAsync(id)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        usuario.Nombres = dto.Nombres;
        usuario.Apellidos = dto.Apellidos;
        usuario.UserName = dto.UserName.Trim();
        usuario.Correo = dto.Correo.Trim();

        await _context.SaveChangesAsync();
    }

    public async Task CambiarEstadoAsync(Guid id, CambiarEstadoUsuarioDto dto)
    {
        var usuario = await _context.Set<Usuario>().FindAsync(id)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        // Regla de negocio restrictiva para el superadministrador
        if (usuario.CodigoUsuario.StartsWith("USR-ADMIN") && dto.Estado != EstadoUsuario.Activo)
            throw new InvalidOperationException("No se puede desactivar al administrador principal del sistema.");

        usuario.Estado = dto.Estado;
        await _context.SaveChangesAsync();
    }

    public async Task AsignarRolAsync(AsignarRolUsuarioDto dto)
    {
        var usuarioExiste = await _context.Set<Usuario>().AnyAsync(x => x.Id == dto.UsuarioId);
        if (!usuarioExiste)
            throw new KeyNotFoundException("Usuario no encontrado.");

        var rolExiste = await _context.Set<Rol>().AnyAsync(x => x.Id == dto.RolId);
        if (!rolExiste)
            throw new KeyNotFoundException("Rol no encontrado.");

        // Validación directa sobre la tabla puente relacional
        var yaTieneRol = await _context.Set<UsuarioRol>()
            .AnyAsync(x => x.UsuarioId == dto.UsuarioId && x.RolId == dto.RolId && x.Activo);

        if (yaTieneRol)
            throw new InvalidOperationException("El usuario ya tiene asignado ese rol.");

        var usuarioRol = new UsuarioRol
        {
            UsuarioId = dto.UsuarioId,
            RolId = dto.RolId,
            FechaAsignacion = DateTime.UtcNow,
            Activo = true
        };

        await _context.Set<UsuarioRol>().AddAsync(usuarioRol);
        await _context.SaveChangesAsync();
    }

    // ==========================================================
    // HELPERS PRIVADOS DE LOGUEO Y LLAVEROS (MANTENIDOS)
    // ==========================================================
    
    // NOTA: Conserva estos dos métodos de soporte si tu controlador de Login o Auth 
    // requiere jalar las credenciales con todo su árbol de permisos mapeado para los Claims del JWT:
    
    public async Task<Usuario?> ObtenerPorCorreoConSeguridadCompletaAsync(string correo)
    {
        return await _context.Set<Usuario>()
            .Include(x => x.UsuarioRoles)
                .ThenInclude(x => x.Rol)
                .ThenInclude(x => x.RolPermisos)
                .ThenInclude(x => x.Permiso)
            .FirstOrDefaultAsync(x => x.Correo == correo);
    }

    public async Task<Usuario?> ObtenerPorUserNameConSeguridadCompletaAsync(string userName)
    {
        return await _context.Set<Usuario>()
            .Include(x => x.UsuarioRoles)
                .ThenInclude(x => x.Rol)
                .ThenInclude(x => x.RolPermisos)
                .ThenInclude(x => x.Permiso)
            .FirstOrDefaultAsync(x => x.UserName == userName);
    }

    private static string GenerarCodigoUsuario() =>
        $"USR-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString("N")[..5].ToUpper()}";
}