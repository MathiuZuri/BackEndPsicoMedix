using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Pagos;

public class PagoResponseDto
{
    public Guid Id { get; set; }
    public string CodigoPago { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public string PacienteNombre { get; set; } = string.Empty;

    public Guid ServicioClinicoId { get; set; }
    public string ServicioNombre { get; set; } = string.Empty;

    public Guid? CitaId { get; set; }
    public Guid? AtencionId { get; set; }

    public decimal MontoTotal { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public decimal MontoAdelanto { get; set; }

    public MetodoPago MetodoPago { get; set; }
    public EstadoPago Estado { get; set; }

    public string? Observacion { get; set; }
    public DateTime FechaPago { get; set; }
}