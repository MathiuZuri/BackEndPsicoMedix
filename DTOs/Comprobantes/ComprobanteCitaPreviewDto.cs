namespace psicomedixMonolito.DTOs.Comprobantes;

public class ComprobanteCitaPreviewDto
{
    public Guid ComprobanteId { get; set; }
    public string CodigoComprobante { get; set; } = string.Empty;

    public Guid CitaId { get; set; }
    public string CodigoCita { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;
    public string? DireccionPaciente { get; set; }

    public Guid DoctorId { get; set; }
    public string Doctor { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;

    public Guid ServicioClinicoId { get; set; }
    public string Servicio { get; set; } = string.Empty;

    public DateOnly FechaCita { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }

    public string EstadoCita { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;

    public DateTime FechaEmision { get; set; }

    public string? Observacion { get; set; }
}