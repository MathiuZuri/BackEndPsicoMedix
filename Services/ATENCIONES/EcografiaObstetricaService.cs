using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Services.imp.ATENCIONES;

namespace psicomedixMonolito.Services.ATENCIONES;

public class EcografiaObstetricaService : IEcografiaObstetricaService
{
    private readonly ApplicationDbContext _context;

    // En el monolito purificado, interactuamos únicamente con el DbContext de la aplicación
    public EcografiaObstetricaService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (ANTES EN REPOSITORY)
    // ==========================================================

    public async Task<IEnumerable<EcografiaObstetricaDto>> ObtenerPorAtencionAsync(Guid atencionId)
    {
        // Consulta directa usando Set<T>() con exclusión de rastreo (AsNoTracking) y ordenamiento descendente
        var entidades = await _context.Set<EcografiaObstetrica>()
            .Where(x => x.AtencionId == atencionId)
            .OrderByDescending(x => x.FechaHora)
            .AsNoTracking()
            .ToListAsync();

        return entidades.Select(e => new EcografiaObstetricaDto
        {
            FechaHora = e.FechaHora,
            DiametroBiparietal = e.DiametroBiparietal,
            CircunferenciaCefalica = e.CircunferenciaCefalica,
            CircunferenciaAbdominal = e.CircunferenciaAbdominal,
            LongitudFemur = e.LongitudFemur,
            PesoFetalEstimado = e.PesoFetalEstimado,
            IndiceLiquidoAmniotico = e.IndiceLiquidoAmniotico,
            PlacentaLocalizacion = e.PlacentaLocalizacion,
            PlacentaGranum = e.PlacentaGranum,
            CircularCordon = e.CircularCordon,
            Conclusiones = e.Conclusiones
        });
    }

    // ==========================================================
    // OPERACIONES DE ESCRITURA DIRECTA
    // ==========================================================

    public async Task<Guid> RegistrarAsync(Guid atencionId, EcografiaObstetricaDto dto)
    {
        var entidad = new EcografiaObstetrica
        {
            Id = Guid.NewGuid(),
            AtencionId = atencionId,
            FechaHora = dto.FechaHora,
            DiametroBiparietal = dto.DiametroBiparietal,
            CircunferenciaCefalica = dto.CircunferenciaCefalica,
            CircunferenciaAbdominal = dto.CircunferenciaAbdominal,
            LongitudFemur = dto.LongitudFemur,
            PesoFetalEstimado = dto.PesoFetalEstimado,
            IndiceLiquidoAmniotico = dto.IndiceLiquidoAmniotico,
            PlacentaLocalizacion = dto.PlacentaLocalizacion,
            PlacentaGranum = dto.PlacentaGranum,
            CircularCordon = dto.CircularCordon,
            Conclusiones = dto.Conclusiones
        };

        // Acceso atómico directo al DbSet correspondiente mediante el contexto
        await _context.Set<EcografiaObstetrica>().AddAsync(entidad);
        await _context.SaveChangesAsync();

        return entidad.Id;
    }
}