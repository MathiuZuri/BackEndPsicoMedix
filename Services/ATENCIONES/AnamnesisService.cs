using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Services.imp.ATENCIONES;

namespace psicomedixMonolito.Services.ATENCIONES;

public class AnamnesisService : IAnamnesisService
{
    private readonly ApplicationDbContext _context;

    // En el monolito, el único punto de contacto con la persistencia es el DbContext
    public AnamnesisService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (ANTES EN REPOSITORY)
    // ==========================================================

    public async Task<AnamnesisDto?> ObtenerPorAtencionAsync(Guid atencionId)
    {
        // Consulta directa usando Set<T>() con exclusión de rastreo (AsNoTracking) para máxima velocidad
        var entidad = await _context.Set<Anamnesis>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AtencionId == atencionId);

        if (entidad == null) return null;

        return new AnamnesisDto
        {
            MotivoConsulta = entidad.MotivoConsulta,
            Gestaciones = entidad.Gestaciones,
            HijosVivos = entidad.HijosVivos,
            Abortos = entidad.Abortos,
            PartosPretermino = entidad.PartosPretermino,
            PartosATermino = entidad.PartosATermino,
            FechaUltimaRegla = entidad.FechaUltimaRegla,
            FechaProbableParto = entidad.FechaProbableParto,
            EdadGestacional = entidad.EdadGestacional,
            Alergias = entidad.Alergias,
            EnfermedadesCronicas = entidad.EnfermedadesCronicas,
            CirugiasPrevias = entidad.CirugiasPrevias,
            AntecedentesAdicionales = entidad.AntecedentesAdicionales
        };
    }

    // ==========================================================
    // OPERACIONES DE ESCRITURA Y VALIDACIÓN DIRECTA
    // ==========================================================

    public async Task<Guid> RegistrarAsync(Guid atencionId, AnamnesisDto dto)
    {
        // 🚀 MEJORA MONOLÍTICA: Verificamos si ya existe (relación 1 a 1) usando un AnyAsync ultraligero directo a SQL
        var existeAnamnesis = await _context.Set<Anamnesis>()
            .AnyAsync(x => x.AtencionId == atencionId);

        if (existeAnamnesis) 
            throw new InvalidOperationException("Esta atención ya tiene una anamnesis registrada.");

        var anamnesis = new Anamnesis
        {
            Id = Guid.NewGuid(),
            AtencionId = atencionId,
            MotivoConsulta = dto.MotivoConsulta,
            Gestaciones = dto.Gestaciones,
            HijosVivos = dto.HijosVivos,
            Abortos = dto.Abortos,
            PartosPretermino = dto.PartosPretermino,
            PartosATermino = dto.PartosATermino,
            FechaUltimaRegla = dto.FechaUltimaRegla,
            FechaProbableParto = dto.FechaProbableParto,
            EdadGestacional = dto.EdadGestacional,
            Alergias = dto.Alergias,
            EnfermedadesCronicas = dto.EnfermedadesCronicas,
            CirugiasPrevias = dto.CirugiasPrevias,
            AntecedentesAdicionales = dto.AntecedentesAdicionales
        };

        await _context.Set<Anamnesis>().AddAsync(anamnesis);
        await _context.SaveChangesAsync();

        return anamnesis.Id;
    }
}