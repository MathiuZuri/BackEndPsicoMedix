using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Services.imp.ATENCIONES;

namespace psicomedixMonolito.Services.ATENCIONES;

public class ExamenFisicoService : IExamenFisicoService
{
    private readonly ApplicationDbContext _context;

    // En el monolito purificado, el servicio interactúa únicamente con el DbContext central
    public ExamenFisicoService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (ANTES EN REPOSITORY)
    // ==========================================================

    public async Task<IEnumerable<ExamenFisicoDto>> ObtenerPorAtencionAsync(Guid atencionId)
    {
        // Traemos todos los exámenes físicos de esta atención directo de la tabla, 
        // ordenados por fecha de más reciente a más antiguo y optimizados con AsNoTracking
        var entidades = await _context.Set<ExamenFisico>()
            .Where(x => x.AtencionId == atencionId)
            .OrderByDescending(x => x.FechaHoraExamen)
            .AsNoTracking()
            .ToListAsync();

        return entidades.Select(e => new ExamenFisicoDto
        {
            FechaHoraExamen = e.FechaHoraExamen,
            Lotep = e.Lotep,
            EstadoGeneral = e.EstadoGeneral,
            EstadoHidratacion = e.EstadoHidratacion,
            EstadoNutricion = e.EstadoNutricion,
            EscalaGlasgow = e.EscalaGlasgow,
            UteroGravido = e.UteroGravido,
            AlturaUterina = e.AlturaUterina,
            SituacionPosicionPresentacion = e.SituacionPosicionPresentacion,
            LatidosCardiacosFetales = e.LatidosCardiacosFetales,
            MovimientosFetales = e.MovimientosFetales,
            TonoUterino = e.TonoUterino,
            DinamicaUterina = e.DinamicaUterina,
            SangradoTv = e.SangradoTv,
            PerdidaLiquidoAmniotico = e.PerdidaLiquidoAmniotico,
            ColorLiquidoAmniotico = e.ColorLiquidoAmniotico,
            TaponMucoso = e.TaponMucoso,
            FlujoVaginal = e.FlujoVaginal,
            PunoPercusionLumbar = e.PunoPercusionLumbar,
            Edemas = e.Edemas,
            ReflejosOsteotendinosos = e.ReflejosOsteotendinosos
        });
    }

    // ==========================================================
    // OPERACIONES DE ESCRITURA DIRECTA
    // ==========================================================

    public async Task<Guid> RegistrarAsync(Guid atencionId, ExamenFisicoDto dto)
    {
        // Manteniendo tu regla de negocio: NO verificamos si existe, porque pueden haber múltiples exámenes físicos en una misma atención
        var entidad = new ExamenFisico
        {
            Id = Guid.NewGuid(),
            AtencionId = atencionId,
            FechaHoraExamen = dto.FechaHoraExamen,
            Lotep = dto.Lotep,
            EstadoGeneral = dto.EstadoGeneral,
            EstadoHidratacion = dto.EstadoHidratacion,
            EstadoNutricion = dto.EstadoNutricion,
            EscalaGlasgow = dto.EscalaGlasgow,
            UteroGravido = dto.UteroGravido,
            AlturaUterina = dto.AlturaUterina,
            SituacionPosicionPresentacion = dto.SituacionPosicionPresentacion,
            LatidosCardiacosFetales = dto.LatidosCardiacosFetales,
            MovimientosFetales = dto.MovimientosFetales,
            TonoUterino = dto.TonoUterino,
            DinamicaUterina = dto.DinamicaUterina,
            SangradoTv = dto.SangradoTv,
            PerdidaLiquidoAmniotico = dto.PerdidaLiquidoAmniotico,
            ColorLiquidoAmniotico = dto.ColorLiquidoAmniotico,
            TaponMucoso = dto.TaponMucoso,
            FlujoVaginal = dto.FlujoVaginal,
            PunoPercusionLumbar = dto.PunoPercusionLumbar,
            Edemas = dto.Edemas,
            ReflejosOsteotendinosos = dto.ReflejosOsteotendinosos
        };

        // Inserción directa en el DbSet utilizando el contexto del monolito
        await _context.Set<ExamenFisico>().AddAsync(entidad);
        await _context.SaveChangesAsync();

        return entidad.Id;
    }
}