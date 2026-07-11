using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Servicios;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;

public class ServicioClinicoService : IServicioClinicoService
{
    private readonly ApplicationDbContext _context;

    // En el monolito, interactuamos directamente con el DbContext centralizado de la aplicación
    public ServicioClinicoService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (ANTES EN INFRASTRUCTURE)
    // ==========================================================

    public async Task<IEnumerable<ServicioClinicoResponseDto>> ObtenerTodosAsync()
    {
        // Consulta directa al DbSet usando Set<T>() con exclusión de rastreo (AsNoTracking)
        var servicios = await _context.Set<ServicioClinico>()
            .AsNoTracking()
            .ToListAsync();

        return servicios.Select(MapearServicio);
    }

    public async Task<IEnumerable<ServicioClinicoResponseDto>> ObtenerActivosAsync()
    {
        // Reemplaza la consulta del antiguo repositorio manteniendo el ordenamiento por Nombre
        var servicios = await _context.Set<ServicioClinico>()
            .Where(x => x.Estado == EstadoServicioClinico.Activo)
            .OrderBy(x => x.Nombre)
            .AsNoTracking()
            .ToListAsync();

        return servicios.Select(MapearServicio);
    }

    public async Task<ServicioClinicoResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        var servicio = await _context.Set<ServicioClinico>().FindAsync(id);
        if (servicio == null) return null;

        return MapearServicio(servicio);
    }

    // ==========================================================
    // HELPERS PRIVADOS Y MAPEADORES INDEPENDIENTES
    // ==========================================================

    private static ServicioClinicoResponseDto MapearServicio(ServicioClinico servicio)
    {
        return new ServicioClinicoResponseDto
        {
            Id = servicio.Id,
            CodigoServicio = servicio.CodigoServicio,
            Nombre = servicio.Nombre,
            Descripcion = servicio.Descripcion,
            CostoBase = servicio.CostoBase,
            DuracionMinutos = servicio.DuracionMinutos,
            RequiereCita = servicio.RequiereCita,
            GeneraHistorial = servicio.GeneraHistorial,
            Estado = servicio.Estado
        };
    }
}