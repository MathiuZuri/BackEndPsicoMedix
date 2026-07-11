namespace psicomedixMonolito.DTOs.Comprobantes;

public class ComprobanteAtencionPreviewDto
{
    public Guid ComprobanteId { get; set; }
    public string CodigoComprobante { get; set; } = string.Empty;

    public Guid AtencionId { get; set; }
    public string CodigoAtencion { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;
    public string? DireccionPaciente { get; set; }

    public Guid DoctorId { get; set; }
    public string Doctor { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;

    public Guid ServicioClinicoId { get; set; }
    public string Servicio { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }
    public DateTime? FechaCierre { get; set; }

    public string MotivoConsulta { get; set; } = string.Empty;
    public string? DiagnosticoResumen { get; set; }
    public string? Indicaciones { get; set; }
    public string? Tratamiento { get; set; }
    public string? Observaciones { get; set; }

    public string EstadoAtencion { get; set; } = string.Empty;

    public decimal CostoFinal { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal SaldoPendiente { get; set; }

    public DateTime FechaEmision { get; set; }
}