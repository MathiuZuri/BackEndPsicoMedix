namespace psicomedixMonolito.DTOs.Finanzas;

public class DetalleEstadoCuentaDto
{
    public Guid PagoId { get; set; }
    public string CodigoPago { get; set; } = string.Empty;

    public Guid? AtencionId { get; set; }
    public string Servicio { get; set; } = string.Empty;

    public decimal MontoTotal { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal SaldoPendiente { get; set; }

    public string EstadoPago { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; }
}