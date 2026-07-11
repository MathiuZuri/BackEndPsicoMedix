namespace psicomedixMonolito.DTOs.Comprobantes;

public class DetalleEstadoCuentaComprobanteDto
{
    public Guid PagoId { get; set; }

    public string CodigoPago { get; set; } = string.Empty;

    public string Servicio { get; set; } = string.Empty;

    public DateTime FechaPago { get; set; }

    public decimal MontoTotal { get; set; }

    public decimal MontoPagado { get; set; }

    public decimal SaldoPendiente { get; set; }

    public string EstadoPago { get; set; } = string.Empty;
}