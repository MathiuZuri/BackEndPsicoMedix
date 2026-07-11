using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.DTOs.Atenciones.Modulos;

namespace psicomedixMonolito.DTOs.Atenciones;

public class RegistrarAtencionDto
{
    [Required(ErrorMessage = "El paciente es obligatorio.")]
    public Guid PacienteId { get; set; }

    [Required(ErrorMessage = "El doctor es obligatorio.")]
    public Guid DoctorId { get; set; }

    [Required(ErrorMessage = "El servicio clínico es obligatorio.")]
    public Guid ServicioClinicoId { get; set; }

    public Guid? CitaId { get; set; }

    public Guid? HistorialClinicoId { get; set; }

    [Required(ErrorMessage = "El costo final es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El costo final debe ser mayor a 0.")]
    public decimal CostoFinal { get; set; }

    public AnamnesisDto? Anamnesis { get; set; }
    public List<ExamenFisicoDto>? ExamenesFisicos { get; set; }
    public List<TactoVaginalDto>? TactosVaginales { get; set; }
    public List<EcografiaObstetricaDto>? Ecografias { get; set; }
    public ImpresionDiagnosticaDto? ImpresionDiagnostica { get; set; }
}