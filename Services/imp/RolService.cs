using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Roles;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;


public class RolService : IRolService
{
    private readonly ApplicationDbContext _context;

    // En el monolito, interactuamos directamente con el DbContext centralizado de la aplicación
    public RolService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (ANTES EN INFRASTRUCTURE)
    // ==========================================================

    public async Task<IEnumerable<RolResponseDto>> ObtenerTodosAsync()
    {
        var roles = await _context.Set<Rol>()
            .AsNoTracking()
            .ToListAsync();

        return roles.Select(x => new RolResponseDto
        {
            Id = x.Id,
            Nombre = x.Nombre,
            Descripcion = x.Descripcion,
            EsSistema = x.EsSistema,
            Activo = x.Activo,
            FechaCreacion = x.FechaCreacion
        });
    }

    public async Task<RolResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        var rol = await _context.Set<Rol>().FindAsync(id);
        if (rol == null) return null;

        return new RolResponseDto
        {
            Id = rol.Id,
            Nombre = rol.Nombre,
            Descripcion = rol.Descripcion,
            EsSistema = rol.EsSistema,
            Activo = rol.Activo,
            FechaCreacion = rol.FechaCreacion
        };
    }

    // ==========================================================
    // OPERACIONES DE MANTENIMIENTO Y ASIGNACIONES
    // ==========================================================

    public async Task<Guid> CrearAsync(CrearRolDto dto)
    {
        // 🚀 MEJORA: Validamos la existencia usando un AnyAsync ultraligero en vez de traer toda la entidad
        var existe = await _context.Set<Rol>().AnyAsync(x => x.Nombre.Trim().ToLower() == dto.Nombre.Trim().ToLower());
        if (existe)
            throw new InvalidOperationException("Ya existe un rol con ese nombre.");

        var rol = new Rol
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion,
            EsSistema = false,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        await _context.Set<Rol>().AddAsync(rol);
        await _context.SaveChangesAsync();

        return rol.Id;
    }

    public async Task ActualizarAsync(Guid id, EditarRolDto dto)
    {
        var rol = await _context.Set<Rol>().FindAsync(id)
            ?? throw new KeyNotFoundException("Rol no encontrado.");

        if (rol.EsSistema)
            throw new InvalidOperationException("No se puede editar un rol del sistema.");

        rol.Nombre = dto.Nombre.Trim();
        rol.Descripcion = dto.Descripcion;
        rol.Activo = dto.Activo;

        // El tracking nativo de Entity Framework detecta los cambios automáticamente
        await _context.SaveChangesAsync();
    }

    public async Task AsignarPermisosAsync(AsignarPermisosRolDto dto)
    {
        var rol = await _context.Set<Rol>().FindAsync(dto.RolId)
            ?? throw new KeyNotFoundException("Rol no encontrado.");

        foreach (var permisoId in dto.PermisosIds.Distinct())
        {
            // Validación directa en la tabla de Permisos sin pasar por repositorio secundario
            var permisoExiste = await _context.Set<Permiso>().AnyAsync(p => p.Id == permisoId);
            if (!permisoExiste)
                throw new KeyNotFoundException("Uno o más permisos no fueron encontrados.");

            // Validación directa en la tabla puente relacional
            var yaTienePermiso = await _context.Set<RolPermiso>()
                .AnyAsync(x => x.RolId == dto.RolId && x.PermisoId == permisoId);

            if (yaTienePermiso)
                continue;

            var nuevoRolPermiso = new RolPermiso
            {
                RolId = dto.RolId,
                PermisoId = permisoId,
                FechaAsignacion = DateTime.UtcNow
            };

            await _context.Set<RolPermiso>().AddAsync(nuevoRolPermiso);
        }

        // Una única llamada guarda todo el lote de asignación de manera transaccional y atómica
        await _context.SaveChangesAsync();
    }
}