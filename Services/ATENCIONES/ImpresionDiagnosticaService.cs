using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Services.imp.ATENCIONES;

namespace psicomedixMonolito.Services.ATENCIONES;

public class ImpresionDiagnosticaService : IImpresionDiagnosticaService
{
    private readonly ApplicationDbContext _context;

    // En el monolito purificado, el servicio interactúa únicamente con el DbContext central
    public ImpresionDiagnosticaService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (ANTES EN REPOSITORY)
    // ==========================================================

    public async Task<ImpresionDiagnosticaDto?> ObtenerPorAtencionAsync(Guid atencionId)
    {
        // Consulta directa al contexto usando Set<T>() y optimizada con AsNoTracking
        var entidad = await _context.Set<ImpresionDiagnostica>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AtencionId == atencionId);

        if (entidad == null) return null;

        return new ImpresionDiagnosticaDto
        {
            DiagnosticoPrincipal = entidad.DiagnosticoPrincipal,
            DiagnosticosSecundarios = entidad.DiagnosticosSecundarios,
            IndicacionesReceta = entidad.IndicacionesReceta,
            FechaProximaCita = entidad.FechaProximaCita,
            MotivoProximaCita = entidad.MotivoProximaCita
        };
    }

    // ==========================================================
    // OPERACIONES DE ESCRITURA Y VALIDACIÓN ATÓMICA DIRECTA
    // ==========================================================

    public async Task<Guid> RegistrarAsync(Guid atencionId, ImpresionDiagnosticaDto dto)
    {
        // 🚀 MEJORA MONOLÍTICA: Verificamos si existe la relación 1 a 1 usando AnyAsync directo a la Base de Datos
        var existeDiagnostico = await _context.Set<ImpresionDiagnostica>()
            .AnyAsync(x => x.AtencionId == atencionId);

        if (existeDiagnostico) 
            throw new InvalidOperationException("Esta atención ya tiene un diagnóstico final.");

        var entidad = new ImpresionDiagnostica
        {
            Id = Guid.NewGuid(),
            AtencionId = atencionId,
            DiagnosticoPrincipal = dto.DiagnosticoPrincipal,
            DiagnosticosSecundarios = dto.DiagnosticosSecundarios,
            IndicacionesReceta = dto.IndicacionesReceta,
            FechaProximaCita = dto.FechaProximaCita,
            MotivoProximaCita = dto.MotivoProximaCita
        };

        // Inserción directa en el DbSet utilizando el contexto del monolito
        await _context.Set<ImpresionDiagnostica>().AddAsync(entidad);
        await _context.SaveChangesAsync();

        return entidad.Id;
    }
}