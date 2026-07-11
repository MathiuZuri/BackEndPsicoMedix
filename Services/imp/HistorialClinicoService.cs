using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Historiales;
using psicomedixMonolito.Models;

// Tu DbContext unificado

namespace psicomedixMonolito.Services.imp;

public class HistorialClinicoService : IHistorialClinicoService
{
    private readonly ApplicationDbContext _context;

    // Inyectamos únicamente el DbContext central del monolito
    public HistorialClinicoService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // CONSULTAS DIRECTAS CON EF CORE (MÉTODOS UNIFICADOS)
    // ==========================================================

    public async Task<HistorialClinicoResponseDto?> ObtenerPorPacienteAsync(Guid pacienteId)
    {
        // 1. Obtener el historial base directo del DbContext
        var historial = await _context.HistorialesClinicos
            .Include(x => x.Paciente)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PacienteId == pacienteId);

        if (historial == null) return null;

        // 2. Obtener la línea de tiempo de detalles con todas sus relaciones anidadas
        var detalles = await _context.HistorialDetalles
            .Include(x => x.Usuario)
            .Include(x => x.Cita)
            .Include(x => x.Atencion)
            .Include(x => x.Pago)
            .Where(x => x.HistorialClinicoId == historial.Id)
            .OrderByDescending(x => x.FechaRegistro)
            .AsNoTracking()
            .ToListAsync();

        return MapearHistorial(historial, detalles);
    }

    public async Task<HistorialClinicoResponseDto?> ObtenerConDetallesAsync(Guid historialId)
    {
        // Obtener todo el expediente integrado en una sola consulta relacional profunda
        var historial = await _context.HistorialesClinicos
            .Include(x => x.Paciente)
            .Include(x => x.Detalles)
                .ThenInclude(x => x.Usuario)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == historialId);

        if (historial == null) return null;

        return MapearHistorial(historial, historial.Detalles);
    }

    // ==========================================================
    // MAPEADORES PRIVADOS (MANTENIDOS 100% COMPATIBLES)
    // ==========================================================

    private static HistorialClinicoResponseDto MapearHistorial(
        HistorialClinico historial,
        IEnumerable<HistorialDetalle> detalles)
    {
        return new HistorialClinicoResponseDto
        {
            Id = historial.Id,
            CodigoHistorial = historial.CodigoHistorial,
            PacienteId = historial.PacienteId,
            PacienteNombre = historial.Paciente == null
                ? string.Empty
                : $"{historial.Paciente.Nombres} {historial.Paciente.Apellidos}",
            PacienteDni = historial.Paciente?.DNI ?? string.Empty,
            FechaApertura = historial.FechaApertura,
            Estado = historial.Estado,
            Detalles = detalles.Select(d => new HistorialDetalleResponseDto
            {
                Id = d.Id,
                CodigoDetalle = d.CodigoDetalle,
                HistorialClinicoId = d.HistorialClinicoId,
                TipoMovimiento = d.TipoMovimiento,
                CitaId = d.CitaId,
                AtencionId = d.AtencionId,
                PagoId = d.PagoId,
                Titulo = d.Titulo,
                Descripcion = d.Descripcion,
                FechaRegistro = d.FechaRegistro,
                UsuarioId = d.UsuarioId,
                UsuarioNombre = d.Usuario == null ? null : $"{d.Usuario.Nombres} {d.Usuario.Apellidos}"
            }).ToList()
        };
    }
}