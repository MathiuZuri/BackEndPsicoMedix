namespace psicomedixMonolito.DTOs.Finanzas;

public class EstadoCuentaPacienteDto
{
    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;

    public decimal TotalFacturado { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal TotalPendiente { get; set; }

    public int CantidadPagos { get; set; }
    public int PagosCompletados { get; set; }
    public int PagosParciales { get; set; }
    public int PagosPendientes { get; set; }

    public List<DetalleEstadoCuentaDto> Detalles { get; set; } = new();
}