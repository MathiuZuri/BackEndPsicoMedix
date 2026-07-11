using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Models.ATENCIONES;
using psicomedixMonolito.Services.imp.ATENCIONES;

namespace psicomedixMonolito.Services.ATENCIONES;

public class TactoVaginalService : ITactoVaginalService
{
    private readonly ApplicationDbContext _context;

    // En el monolito purificado, el servicio interactúa únicamente con el DbContext central
    public TactoVaginalService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (ANTES EN REPOSITORY)
    // ==========================================================

    public async Task<IEnumerable<TactoVaginalDto>> ObtenerPorAtencionAsync(Guid atencionId)
    {
        // Consulta directa al contexto usando Set<T>() con ordenamiento descendente y optimizado con AsNoTracking
        var entidades = await _context.Set<TactoVaginal>()
            .Where(x => x.AtencionId == atencionId)
            .OrderByDescending(x => x.FechaHora)
            .AsNoTracking()
            .ToListAsync();

        return entidades.Select(t => new TactoVaginalDto
        {
            FechaHora = t.FechaHora,
            Dilatacion = t.Dilatacion,
            Borramiento = t.Borramiento,
            AlturaPresentacion = t.AlturaPresentacion,
            MembranasOvulares = t.MembranasOvulares,
            ColorLiquido = t.ColorLiquido,
            Pelvis = t.Pelvis,
            VariedadPresentacion = t.VariedadPresentacion
        });
    }

    // ==========================================================
    // OPERACIONES DE ESCRITURA DIRECTA
    // ==========================================================

    public async Task<Guid> RegistrarAsync(Guid atencionId, TactoVaginalDto dto)
    {
        // En este módulo pueden haber múltiples registros en una misma atención (monitoreo de labor de parto)
        var entidad = new TactoVaginal
        {
            Id = Guid.NewGuid(),
            AtencionId = atencionId,
            FechaHora = dto.FechaHora,
            Dilatacion = dto.Dilatacion,
            Borramiento = dto.Borramiento,
            AlturaPresentacion = dto.AlturaPresentacion,
            MembranasOvulares = dto.MembranasOvulares,
            ColorLiquido = dto.ColorLiquido,
            Pelvis = dto.Pelvis,
            VariedadPresentacion = dto.VariedadPresentacion
        };

        // Inserción directa en el DbSet utilizando el contexto del monolito
        await _context.Set<TactoVaginal>().AddAsync(entidad);
        await _context.SaveChangesAsync();

        return entidad.Id;
    }
}