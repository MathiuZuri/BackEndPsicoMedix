using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Models;

public class NotificacionCita
{
    // esto es exclusivo de evolution api
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CitaId { get; set; }
    public Cita Cita { get; set; } = null!;

    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public string TelefonoDestino { get; set; } = string.Empty;

    public CanalNotificacion Canal { get; set; } = CanalNotificacion.WhatsApp;

    public string Mensaje { get; set; } = string.Empty;

    public DateTime FechaProgramadaEnvio { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public EstadoNotificacion Estado { get; set; } = EstadoNotificacion.Pendiente;

    public int Intentos { get; set; }

    public string? Error { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaActualizacion { get; set; }
}