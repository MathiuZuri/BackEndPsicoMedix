namespace psicomedixMonolito.DTOs.Finanzas;

public class PagoFinanzasDto
{
    public Guid PagoId { get; set; }
    public string CodigoPago { get; set; } = string.Empty;

    public Guid? PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;

    public Guid? AtencionId { get; set; }
    public string Servicio { get; set; } = string.Empty;

    public decimal MontoTotal { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal SaldoPendiente { get; set; }

    public string EstadoPago { get; set; } = string.Empty;
    public string MetodoPago { get; set; } = string.Empty;

    public DateTime FechaPago { get; set; }
    public string RegistradoPor { get; set; } = string.Empty;
}