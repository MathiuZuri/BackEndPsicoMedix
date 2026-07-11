namespace psicomedixMonolito.DTOs.Comprobantes;

public class ComprobanteDto
{
    public Guid Id { get; set; }

    public string CodigoComprobante { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public int Numero { get; set; }

    public string TipoComprobante { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string FormatoImpresion { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;

    public string TipoDocumentoPaciente { get; set; } = string.Empty;
    public string NumeroDocumentoPaciente { get; set; } = string.Empty;
    public string? DireccionPaciente { get; set; }

    public Guid? PagoId { get; set; }
    public Guid? CitaId { get; set; }
    public Guid? AtencionId { get; set; }
    public Guid? HistorialClinicoId { get; set; }

    public decimal Subtotal { get; set; }
    public decimal TasaImpuesto { get; set; }
    public decimal MontoImpuesto { get; set; }
    public decimal Total { get; set; }

    public DateTime FechaEmision { get; set; }

    public Guid UsuarioEmisionId { get; set; }
    public string? UsuarioEmision { get; set; }

    public DateTime? FechaAnulacion { get; set; }
    public Guid? UsuarioAnulacionId { get; set; }
    public string? UsuarioAnulacion { get; set; }
    public string? MotivoAnulacion { get; set; }

    public string? Observacion { get; set; }

    public List<ComprobanteDetalleDto> Detalles { get; set; } = new();
}