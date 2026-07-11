namespace psicomedixMonolito.DTOs.Finanzas;

public class EstadoPagoAtencionDto
{
    public Guid AtencionId { get; set; }

    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;

    public string Servicio { get; set; } = string.Empty;

    public decimal MontoTotal { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal SaldoReal { get; set; }
    public decimal Sobrepago { get; set; }

    public bool TieneDeuda { get; set; }
    public bool TieneSobrepago { get; set; }

    public string EstadoFinanciero { get; set; } = string.Empty;

    public DateTime? FechaPrimerPago { get; set; }
    public DateTime? FechaUltimoPago { get; set; }

    public int CantidadPagos { get; set; }

    public List<PagoFinanzasDto> Pagos { get; set; } = new();
    
    public decimal DeudaTotal => SaldoReal;
    
    public string TipoUltimoPago { get; set; } = string.Empty;
}