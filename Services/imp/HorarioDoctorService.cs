using Microsoft.EntityFrameworkCore;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.DTOs.Horarios;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;

// Tu DbContext unificado

namespace psicomedixMonolito.Services.imp;

public class HorarioDoctorService : IHorarioDoctorService
{
    private readonly ApplicationDbContext _context;

    // En el monolito, el único punto de contacto con los datos es el DbContext de la aplicación
    public HorarioDoctorService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ==========================================================
    // GENERACIÓN DE MATRIZ SEMANAL (OPTIMIZADA A NIVEL DE SQL)
    // ==========================================================
    public async Task<MatrizSemanalDto> ObtenerMatrizSemanalAsync(Guid doctorId, DateOnly fechaReferencia)
    {
        var doctor = await _context.Set<Doctor>().FindAsync(doctorId)
            ?? throw new KeyNotFoundException("Doctor no encontrado.");

        // Calcular los límites de la semana (Lunes a Domingo)
        int diasDiferencia = (int)fechaReferencia.DayOfWeek - (int)DayOfWeek.Monday;
        if (diasDiferencia < 0) diasDiferencia += 7;
        
        var fechaLunes = fechaReferencia.AddDays(-diasDiferencia);
        var fechaDomingo = fechaLunes.AddDays(6);

        // 1. Obtener la plantilla de turnos configurados para el doctor directo de la tabla
        var turnosPlantilla = await _context.Set<HorarioDoctor>()
            .Include(x => x.Doctor)
            .Where(x => x.DoctorId == doctorId)
            .OrderBy(x => x.DiaSemana)
            .ThenBy(x => x.HoraInicio)
            .AsNoTracking()
            .ToListAsync();
        
        // 🚀 MEJORA MONOLÍTICA CRÍTICA: Filtramos las citas por DoctorId y rango de fechas directamente en SQL
        var citasDoctor = await _context.Set<Cita>()
            .Include(x => x.Paciente)
            .Include(x => x.ServicioClinico)
            .Where(x => x.DoctorId == doctorId && 
                        x.Fecha >= fechaLunes && 
                        x.Fecha <= fechaDomingo &&
                        x.Estado != EstadoCita.Cancelada && 
                        x.Estado != EstadoCita.Eliminada)
            .AsNoTracking()
            .ToListAsync();

        var matriz = new MatrizSemanalDto
        {
            DoctorId = doctorId,
            DoctorNombre = $"{doctor.Nombres} {doctor.Apellidos}",
            Especialidad = doctor.Especialidad,
            FechaInicioSemana = fechaLunes,
            FechaFinSemana = fechaDomingo
        };

        var horaApertura = new TimeOnly(8, 0);
        var horaCierre = new TimeOnly(18, 0);
        var intervaloBase = TimeSpan.FromMinutes(30);
        var horaActual = horaApertura;

        // Construcción de la grilla de tiempos
        while (horaActual < horaCierre)
        {
            var horaSiguiente = horaActual.Add(intervaloBase);
            var fila = new FilaMatrizDto
            {
                HoraInicio = horaActual,
                RangoHora = $"{horaActual:HH:mm} - {horaSiguiente:HH:mm}"
            };

            for (int i = 0; i < 7; i++)
            {
                var diaAnalizar = (DayOfWeek)(((int)DayOfWeek.Monday + i) % 7);
                var fechaCelda = fechaLunes.AddDays(i);

                var celda = new CeldaMatrizDto
                {
                    DiaSemana = diaAnalizar,
                    FechaCelda = fechaCelda,
                    Estado = "FueraHorario"
                };

                // Evaluar si el bloque horario cae dentro de la plantilla vigente del doctor
                var plantillaDia = turnosPlantilla.FirstOrDefault(x => x.DiaSemana == diaAnalizar && x.Activo &&
                    fechaCelda >= x.FechaInicioVigencia && (!x.FechaFinVigencia.HasValue || fechaCelda <= x.FechaFinVigencia.Value));

                if (plantillaDia != null && horaActual >= plantillaDia.HoraInicio && horaSiguiente <= plantillaDia.HoraFin)
                {
                    celda.Estado = "Disponible";
                }

                // Evaluar si hay un cruce con una cita guardada en la base de datos
                var citaBloque = citasDoctor.FirstOrDefault(x => x.Fecha == fechaCelda &&
                    ((horaActual >= x.HoraInicio && horaActual < x.HoraFin) || (x.HoraInicio >= horaActual && x.HoraInicio < horaSiguiente)));

                if (citaBloque != null)
                {
                    celda.Estado = "Ocupado";
                    celda.CitaId = citaBloque.Id;
                    celda.CodigoCita = citaBloque.CodigoCita;
                    celda.PacienteNombre = citaBloque.Paciente != null ? $"{citaBloque.Paciente.Nombres} {citaBloque.Paciente.Apellidos}" : "Paciente Anónimo";
                    celda.ServicioNombre = citaBloque.ServicioClinico?.Nombre ?? "Consulta General";
                }

                fila.CeldasDias.Add(celda);
            }

            matriz.Filas.Add(fila);
            horaActual = horaSiguiente;
        }

        return matriz;
    }

    // ==========================================================
    // MANTENIMIENTOS Y CONSULTAS DIRECTAS CON EF CORE
    // ==========================================================
    
    public async Task<IEnumerable<HorarioDoctorResponseDto>> ObtenerTodosAsync()
    {
        var horarios = await _context.Set<HorarioDoctor>()
            .Include(x => x.Doctor)
            .OrderBy(x => x.DiaSemana)
            .ThenBy(x => x.HoraInicio)
            .AsNoTracking()
            .ToListAsync();

        return horarios.Select(MapearHorario);
    }

    public async Task<IEnumerable<HorarioDoctorResponseDto>> ObtenerPorDoctorAsync(Guid doctorId)
    {
        var horarios = await _context.Set<HorarioDoctor>()
            .Include(x => x.Doctor)
            .Where(x => x.DoctorId == doctorId)
            .OrderBy(x => x.DiaSemana)
            .ThenBy(x => x.HoraInicio)
            .AsNoTracking()
            .ToListAsync();

        return horarios.Select(MapearHorario);
    }

    public async Task<Guid> CrearAsync(CrearHorarioDoctorDto dto)
    {
        var doctor = await _context.Set<Doctor>().FindAsync(dto.DoctorId)
            ?? throw new KeyNotFoundException("Doctor no encontrado.");

        if (dto.HoraFin <= dto.HoraInicio)
            throw new InvalidOperationException("La hora de fin debe ser mayor que la hora de inicio.");
        
        if (dto.FechaFinVigencia.HasValue && dto.FechaFinVigencia.Value < dto.FechaInicioVigencia)
            throw new InvalidOperationException("La fecha de fin de vigencia no puede ser menor que la fecha de inicio.");

        var horario = new HorarioDoctor
        {
            Id = Guid.NewGuid(),
            DoctorId = dto.DoctorId,
            DiaSemana = dto.DiaSemana,
            HoraInicio = dto.HoraInicio,
            HoraFin = dto.HoraFin,
            FechaInicioVigencia = dto.FechaInicioVigencia,
            FechaFinVigencia = dto.FechaFinVigencia,
            Activo = true
        };

        await _context.Set<HorarioDoctor>().AddAsync(horario);
        await _context.SaveChangesAsync();

        return horario.Id;
    }

    public async Task ActualizarAsync(Guid id, EditarHorarioDoctorDto dto)
    {
        var horario = await _context.Set<HorarioDoctor>().FindAsync(id)
            ?? throw new KeyNotFoundException("Horario no encontrado.");

        if (dto.HoraFin <= dto.HoraInicio)
            throw new InvalidOperationException("La hora de fin debe ser mayor que la hora de inicio.");
        
        if (dto.FechaFinVigencia.HasValue && dto.FechaFinVigencia.Value < dto.FechaInicioVigencia)
            throw new InvalidOperationException("La fecha de fin de vigencia no puede ser menor que la fecha de inicio.");

        horario.DiaSemana = dto.DiaSemana;
        horario.HoraInicio = dto.HoraInicio;
        horario.HoraFin = dto.HoraFin;
        horario.FechaInicioVigencia = dto.FechaInicioVigencia;
        horario.FechaFinVigencia = dto.FechaFinVigencia;
        horario.Activo = dto.Activo;

        // El tracking de cambios de EF Core se encarga de actualizar el objeto en memoria automáticamente
        await _context.SaveChangesAsync();
    }

    // ==========================================================
    // HELPERS DE ENLACE Y MAPEO GENERAL
    // ==========================================================
    private static HorarioDoctorResponseDto MapearHorario(HorarioDoctor horario)
    {
        return new HorarioDoctorResponseDto
        {
            Id = horario.Id,
            DoctorId = horario.DoctorId,
            DoctorNombre = horario.Doctor == null ? string.Empty : $"{horario.Doctor.Nombres} {horario.Doctor.Apellidos}",
            DiaSemana = horario.DiaSemana,
            HoraInicio = horario.HoraInicio,
            HoraFin = horario.HoraFin,
            FechaInicioVigencia = horario.FechaInicioVigencia,
            FechaFinVigencia = horario.FechaFinVigencia,
            Activo = horario.Activo
        };
    }
}