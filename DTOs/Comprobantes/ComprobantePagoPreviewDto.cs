namespace psicomedixMonolito.DTOs.Comprobantes;

public class ComprobantePagoPreviewDto
{
    public Guid ComprobanteId { get; set; }
    public string CodigoComprobante { get; set; } = string.Empty;

    public Guid PagoId { get; set; }
    public string CodigoPago { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;
    public string? DireccionPaciente { get; set; }

    public Guid? AtencionId { get; set; }
    public string? CodigoAtencion { get; set; }

    public Guid? CitaId { get; set; }
    public string? CodigoCita { get; set; }

    public string Servicio { get; set; } = string.Empty;

    public decimal MontoPagado { get; set; }

    public decimal Subtotal { get; set; }
    public decimal TasaImpuesto { get; set; }
    public decimal MontoImpuesto { get; set; }
    public decimal Total { get; set; }

    public string MetodoPago { get; set; } = string.Empty;
    public string EstadoPago { get; set; } = string.Empty;

    public DateTime FechaPago { get; set; }
    public DateTime FechaEmision { get; set; }

    public string? Observacion { get; set; }

    public List<ComprobanteDetalleDto> Detalles { get; set; } = new();
}