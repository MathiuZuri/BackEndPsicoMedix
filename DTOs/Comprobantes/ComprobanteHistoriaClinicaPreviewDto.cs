namespace psicomedixMonolito.DTOs.Comprobantes;

public class ComprobanteHistoriaClinicaPreviewDto
{
    public Guid HistorialClinicoId { get; set; }

    public Guid PacienteId { get; set; }
    public string Paciente { get; set; } = string.Empty;
    public string DniPaciente { get; set; } = string.Empty;
    public string CodigoPaciente { get; set; } = string.Empty;

    public DateTime FechaRegistroPaciente { get; set; }

    public int CantidadAtenciones { get; set; }
    public int CantidadCitas { get; set; }

    public List<ComprobanteAtencionPreviewDto> Atenciones { get; set; } = new();
    public List<ComprobanteCitaPreviewDto> Citas { get; set; } = new();
}