using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using psicomedixMonolito.DbFiles.Data;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models;
using psicomedixMonolito.Services.imp.WhastAppImp;
using psicomedixMonolito.Utils.Configurations;

namespace psicomedixMonolito.Services.Background;

public class RecordatorioCitasBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WhatsAppOptions _options;
    private readonly ILogger<RecordatorioCitasBackgroundService> _logger;

    public RecordatorioCitasBackgroundService(
        IServiceScopeFactory scopeFactory,
        IOptions<WhatsAppOptions> options,
        ILogger<RecordatorioCitasBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation("Servicio de recordatorios WhatsApp deshabilitado.");
            return;
        }

        _logger.LogInformation(
            "RecordatorioCitasBackgroundService iniciado. Intervalo: {CheckIntervalMinutes} min, HorasAntes: {ReminderHoursBefore}",
            _options.CheckIntervalMinutes,
            _options.ReminderHoursBefore);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcesarRecordatoriosAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando recordatorios de citas.");
            }

            await Task.Delay(TimeSpan.FromMinutes(_options.CheckIntervalMinutes), stoppingToken);
        }
    }

    private async Task ProcesarRecordatoriosAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        
        // Resolvemos el DbContext y el servicio HTTP de WhatsApp directamente en el scope
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var whatsAppService = scope.ServiceProvider.GetRequiredService<INotificacionWhatsAppService>();

        var ahoraUtc = DateTime.UtcNow;
        var desde = ahoraUtc.AddHours(_options.ReminderHoursBefore - 1);
        var hasta = ahoraUtc.AddHours(_options.ReminderHoursBefore + 1);

        // Cloramos las fechas relativas para buscar citas del día de hoy y de mañana
        var hoy = DateOnly.FromDateTime(DateTime.Now);
        var manana = hoy.AddDays(1);

        // 1. Consulta directa a la tabla de Citas (Sustituye a citaRepository.ObtenerCitasParaRecordatorioAsync)
        var citas = await context.Set<Cita>()
            .Include(x => x.Paciente)
            .Include(x => x.Doctor)
            .Include(x => x.ServicioClinico)
            .Where(x =>
                x.Fecha >= hoy &&
                x.Fecha <= manana &&
                x.Paciente.Celular != null &&
                x.Paciente.Celular != "" &&
                (
                    x.Estado == EstadoCita.Pendiente ||
                    x.Estado == EstadoCita.Confirmada ||
                    x.Estado == EstadoCita.Reprogramada
                ))
            .OrderBy(x => x.Fecha)
            .ThenBy(x => x.HoraInicio)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Citas encontradas para evaluar recordatorio: {Cantidad}", citas.Count);

        foreach (var cita in citas)
        {
            if (cancellationToken.IsCancellationRequested) return;

            // 2. Validación de existencia directa contra la tabla de Notificaciones
            var yaExiste = await context.Set<NotificacionCita>()
                .AnyAsync(x =>
                    x.CitaId == cita.Id &&
                    x.Canal == CanalNotificacion.WhatsApp &&
                    x.Estado != EstadoNotificacion.Cancelado, cancellationToken);

            if (yaExiste) continue;

            var fechaHoraCita = cita.Fecha.ToDateTime(cita.HoraInicio);
            var fechaProgramada = fechaHoraCita.AddHours(-_options.ReminderHoursBefore);
            var mensaje = ConstruirMensaje(cita);

            var notificacion = new NotificacionCita
            {
                Id = Guid.NewGuid(),
                CitaId = cita.Id,
                PacienteId = cita.PacienteId,
                TelefonoDestino = cita.Paciente.Celular!,
                Canal = CanalNotificacion.WhatsApp,
                Mensaje = mensaje,
                FechaProgramadaEnvio = fechaProgramada.ToUniversalTime(),
                Estado = EstadoNotificacion.Pendiente,
                Intentos = 0,
                FechaCreacion = DateTime.UtcNow
            };

            await context.Set<NotificacionCita>().AddAsync(notificacion, cancellationToken);
        }

        // Guardamos las nuevas notificaciones en cola antes de proceder al envío masivo
        await context.SaveChangesAsync(cancellationToken);

        // 3. Obtener notificaciones pendientes directo del contexto
        var pendientes = await context.Set<NotificacionCita>()
            .Include(x => x.Cita)
                .ThenInclude(c => c!.Paciente)
            .Include(x => x.Cita)
                .ThenInclude(c => c!.Doctor)
            .Where(x =>
                x.Estado == EstadoNotificacion.Pendiente &&
                x.FechaProgramadaEnvio <= ahoraUtc &&
                x.Intentos < _options.MaxIntentos)
            .OrderBy(x => x.FechaProgramadaEnvio)
            .ToListAsync(cancellationToken);

        // 4. Proceso de envío mediante la Evolution API
        foreach (var notificacion in pendientes)
        {
            if (cancellationToken.IsCancellationRequested) return;

            try
            {
                notificacion.Intentos++;
                notificacion.FechaActualizacion = DateTime.UtcNow;

                await whatsAppService.EnviarMensajeAsync(
                    notificacion.TelefonoDestino,
                    notificacion.Mensaje,
                    cancellationToken);

                notificacion.Estado = EstadoNotificacion.Enviado;
                notificacion.FechaEnvio = DateTime.UtcNow;
                notificacion.Error = null;
            }
            catch (Exception ex)
            {
                notificacion.Error = ex.Message;
                notificacion.FechaActualizacion = DateTime.UtcNow;

                if (notificacion.Intentos >= _options.MaxIntentos)
                    notificacion.Estado = EstadoNotificacion.Fallido;

                _logger.LogWarning(ex, "No se pudo enviar recordatorio de cita. NotificacionId: {NotificacionId}", notificacion.Id);
            }

            // Al estar las entidades trackeadas en el contexto del ámbito, solo mandamos a confirmar cambios
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private static string ConstruirMensaje(Cita cita)
    {
        var paciente = $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}".Trim().ToUpper();
        var doctor = $"{cita.Doctor.Nombres} {cita.Doctor.Apellidos}".Trim().ToUpper();
        var fechaHora = cita.Fecha.ToDateTime(cita.HoraInicio);
        var cultura = new System.Globalization.CultureInfo("es-PE");

        var fechaFormateada = fechaHora.ToString("dddd dd/MM/yyyy", cultura);
        var horaFormateada = cita.HoraInicio.ToString("HH:mm");

        // ✅ Modificado con el nombre del nuevo sistema unificado
        return
            "Recordatorio de cita\n\n" +
            "PSICOMEDIX 🧠🏥\n" +
            $"Estimado(a) {paciente}, tiene agendada una cita:\n\n" +
            $"📆 Fecha: {fechaFormateada}\n\n" +
            $"⏰ Hora: {horaFormateada}\n\n" +
            $"🏥 Especialista: {doctor}\n\n" +
            "Estaremos pendientes a su confirmación.\n" +
            "¡Esperamos su asistencia!\n\n" +
            "Horario de atención: De lunes a viernes, turno mañana 8:30 a. m. a 1:00 p. m. - turno tarde 3:00 p. m. a 8:00 p. m.";
    }
}