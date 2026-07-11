using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Citas;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Services.imp;

public class CitaService : ICitaService
{
    private readonly ApplicationDbContext _context;

    // El monolito ahora requiere únicamente el DbContext centralizado
    public CitaService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ========== CONSULTAS DIRECTAS CON EF CORE ==========

    public async Task<IEnumerable<CitaResponseDto>> ObtenerTodasAsync()
    {
        var citas = await _context.Set<Cita>()
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
            .Include(x => x.ServicioClinico)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.HoraInicio)
            .AsNoTracking()
            .ToListAsync();

        return citas.Select(MapearCita);
    }

    public async Task<CitaResponseDto?> ObtenerPorIdAsync(Guid id)
    {
        var cita = await _context.Set<Cita>()
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
            .Include(x => x.ServicioClinico)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (cita == null) return null;
        return MapearCita(cita);
    }

    public async Task<IEnumerable<CitaResponseDto>> ObtenerPorPacienteAsync(Guid pacienteId)
    {
        var citas = await _context.Set<Cita>()
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
            .Include(x => x.ServicioClinico)
            .Where(x => x.PacienteId == pacienteId)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.HoraInicio)
            .AsNoTracking()
            .ToListAsync();

        return citas.Select(MapearCita);
    }

    public async Task<IEnumerable<CitaResponseDto>> ObtenerPorDoctorAsync(Guid doctorId)
    {
        var citas = await _context.Set<Cita>()
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
            .Include(x => x.ServicioClinico)
            .Where(x => x.DoctorId == doctorId)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.HoraInicio)
            .AsNoTracking()
            .ToListAsync();

        return citas.Select(MapearCita);
    }

    // ========== OPERACIONES DE ESCRITURA Y TRANSACCIONES ==========

    public async Task<Guid> CrearAsync(CrearCitaDto dto, Guid usuarioId)
    {
        if (dto.Fecha < DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException("No se puede programar una cita en una fecha pasada.");

        if (dto.HoraFin <= dto.HoraInicio)
            throw new InvalidOperationException("La hora de fin debe ser mayor que la hora de inicio.");

        var paciente = await _context.Set<Paciente>().FindAsync(dto.PacienteId)
            ?? throw new KeyNotFoundException("Paciente no encontrado.");

        var doctor = await _context.Set<Doctor>().FindAsync(dto.DoctorId)
            ?? throw new KeyNotFoundException("Doctor no encontrado.");

        var servicio = await _context.Set<ServicioClinico>().FindAsync(dto.ServicioClinicoId)
            ?? throw new KeyNotFoundException("Servicio clínico no encontrado.");

        var existeCruce = await ExisteInterferenciaHorarioAsync(
            dto.DoctorId,
            dto.Fecha,
            dto.HoraInicio,
            dto.HoraFin);

        if (existeCruce)
            throw new InvalidOperationException("El doctor ya tiene una cita en ese horario.");

        var cita = new Cita
        {
            Id = Guid.NewGuid(),
            CodigoCita = GenerarCodigo("CIT", paciente.DNI),
            PacienteId = dto.PacienteId,
            DoctorId = dto.DoctorId,
            ServicioClinicoId = dto.ServicioClinicoId,
            HorarioDoctorId = dto.HorarioDoctorId,
            Fecha = dto.Fecha,
            HoraInicio = dto.HoraInicio,
            HoraFin = dto.HoraFin,
            Motivo = dto.Motivo,
            Observaciones = dto.Observaciones,
            Estado = EstadoCita.Pendiente,
            FechaRegistro = DateTime.UtcNow,
            UsuarioRegistroId = usuarioId // Recibido por parámetro directo
        };

        await _context.Set<Cita>().AddAsync(cita);

        var historial = await _context.Set<HistorialClinico>()
            .FirstOrDefaultAsync(x => x.PacienteId == dto.PacienteId);
            
        if (historial != null)
        {
            var detalle = new HistorialDetalle
            {
                Id = Guid.NewGuid(),
                CodigoDetalle = GenerarCodigo(servicio.CodigoServicio, paciente.DNI),
                HistorialClinicoId = historial.Id,
                CitaId = cita.Id,
                TipoMovimiento = TipoMovimientoHistorial.CitaProgramada,
                Titulo = "Cita programada",
                Descripcion = $"Se programó una cita para el servicio {servicio.Nombre}.",
                FechaRegistro = DateTime.UtcNow,
                UsuarioId = usuarioId
            };
            await _context.Set<HistorialDetalle>().AddAsync(detalle);
        }

        await _context.SaveChangesAsync();
        return cita.Id;
    }

    public async Task ReprogramarAsync(Guid id, ReprogramarCitaDto dto)
    {
        var cita = await _context.Set<Cita>().FindAsync(id)
            ?? throw new KeyNotFoundException("Cita no encontrada.");

        var existeCruce = await ExisteInterferenciaHorarioAsync(
            dto.DoctorId,
            dto.NuevaFecha,
            dto.NuevaHoraInicio,
            dto.NuevaHoraFin,
            id);

        if (existeCruce)
            throw new InvalidOperationException("El doctor ya tiene una cita en ese nuevo horario.");
        
        if (dto.NuevaFecha < DateOnly.FromDateTime(DateTime.Today))
            throw new InvalidOperationException("No se puede reprogramar una cita en una fecha pasada.");

        if (dto.NuevaHoraFin <= dto.NuevaHoraInicio)
            throw new InvalidOperationException("La hora de fin debe ser mayor que la hora de inicio.");

        cita.DoctorId = dto.DoctorId;
        cita.HorarioDoctorId = dto.HorarioDoctorId;
        cita.Fecha = dto.NuevaFecha;
        cita.HoraInicio = dto.NuevaHoraInicio;
        cita.HoraFin = dto.NuevaHoraFin;
        cita.Estado = EstadoCita.Reprogramada;
        cita.Observaciones = dto.MotivoReprogramacion;

        await _context.SaveChangesAsync();
    }

    public async Task CancelarAsync(Guid id, CancelarCitaDto dto)
    {
        var cita = await _context.Set<Cita>().FindAsync(id)
            ?? throw new KeyNotFoundException("Cita no encontrada.");

        cita.Estado = EstadoCita.Cancelada;
        cita.Observaciones = dto.MotivoCancelacion;

        await _context.SaveChangesAsync();
    }

    // ========== MÉTODOS DE SOPORTE INTERNOS ==========

    private async Task<bool> ExisteInterferenciaHorarioAsync(
        Guid doctorId,
        DateOnly fecha,
        TimeOnly horaInicio,
        TimeOnly horaFin,
        Guid? citaIdExcluir = null)
    {
        var query = _context.Set<Cita>()
            .Where(x =>
                x.DoctorId == doctorId &&
                x.Fecha == fecha &&
                x.Estado != EstadoCita.Cancelada &&
                x.Estado != EstadoCita.Eliminada);

        if (citaIdExcluir.HasValue)
        {
            query = query.Where(x => x.Id != citaIdExcluir.Value);
        }

        return await query.AnyAsync(x =>
            horaInicio < x.HoraFin &&
            horaFin > x.HoraInicio);
    }

    private static CitaResponseDto MapearCita(Cita cita)
    {
        return new CitaResponseDto
        {
            Id = cita.Id,
            CodigoCita = cita.CodigoCita,
            PacienteId = cita.PacienteId,
            PacienteNombre = cita.Paciente == null ? string.Empty : $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}",
            DoctorId = cita.DoctorId,
            DoctorNombre = cita.Doctor == null ? string.Empty : $"{cita.Doctor.Nombres} {cita.Doctor.Apellidos}",
            ServicioClinicoId = cita.ServicioClinicoId,
            ServicioNombre = cita.ServicioClinico?.Nombre ?? string.Empty,
            Fecha = cita.Fecha,
            HoraInicio = cita.HoraInicio,
            HoraFin = cita.HoraFin,
            Motivo = cita.Motivo,
            Observaciones = cita.Observaciones,
            Estado = cita.Estado,
            FechaRegistro = cita.FechaRegistro
        };
    }

    private static string GenerarCodigo(string prefijo, string dni)
    {
        return $"{Guid.NewGuid().ToString("N")[..5].ToUpper()}-{prefijo}-{DateTime.UtcNow:yyyy}-{dni}";
    }
}