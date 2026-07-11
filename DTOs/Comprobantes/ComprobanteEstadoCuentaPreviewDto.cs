namespace psicomedixMonolito.DTOs.Comprobantes;

public class ComprobanteEstadoCuentaPreviewDto
{
    public Guid ComprobanteId { get; set; }
    public string CodigoComprobante { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;
    public string? DireccionPaciente { get; set; }

    public decimal TotalFacturado { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal TotalPendiente { get; set; }

    public DateTime FechaEmision { get; set; }

    public List<DetalleEstadoCuentaComprobanteDto> Detalles { get; set; } = new();
}