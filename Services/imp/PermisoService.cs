using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Permisos;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;

public class PermisoService : IPermisoService
{
    private readonly ApplicationDbContext _context;

    // En el monolito, consumimos directamente el DbContext central de la aplicación
    public PermisoService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (ANTES EN INFRASTRUCTURE)
    // ==========================================================

    public async Task<IEnumerable<PermisoResponseDto>> ObtenerTodosAsync()
    {
        // Consulta directa al DbSet usando Set<T>() con exclusión de rastreo (AsNoTracking)
        var permisos = await _context.Set<Permiso>()
            .AsNoTracking()
            .ToListAsync();

        return permisos.Select(x => new PermisoResponseDto
        {
            Id = x.Id,
            Codigo = x.Codigo,
            Nombre = x.Nombre,
            Modulo = x.Modulo,
            Descripcion = x.Descripcion,
            Activo = x.Activo
        });
    }
}