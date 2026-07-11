namespace psicomedixMonolito.DTOs.Finanzas;

public class AjusteFinancieroDto
{
    public Guid Id { get; set; }

    public Guid PagoId { get; set; }
    public string CodigoPago { get; set; } = string.Empty;

    public Guid? AtencionId { get; set; }

    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;

    public string TipoAjuste { get; set; } = string.Empty;
    public decimal MontoAjuste { get; set; }

    public string Motivo { get; set; } = string.Empty;
    public string? Observacion { get; set; }

    public string RegistradoPor { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
}