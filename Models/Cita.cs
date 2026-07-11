using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class Cita
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoCita { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public Guid ServicioClinicoId { get; set; }
    public ServicioClinico ServicioClinico { get; set; } = null!;

    public Guid? HorarioDoctorId { get; set; }
    public HorarioDoctor? HorarioDoctor { get; set; }

    public DateOnly Fecha { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }

    public string Motivo { get; set; } = string.Empty;
    public string? Observaciones { get; set; }

    public EstadoCita Estado { get; set; } = EstadoCita.Pendiente;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public Guid? UsuarioRegistroId { get; set; }
    public Usuario? UsuarioRegistro { get; set; }

    public Atencion? Atencion { get; set; }
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<HistorialDetalle> HistorialDetalles { get; set; } = new List<HistorialDetalle>();
    public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
    
    // esto es exclusivo de evolution api, no incluir al sistema
    public ICollection<NotificacionCita> Notificaciones { get; set; } = new List<NotificacionCita>();
}