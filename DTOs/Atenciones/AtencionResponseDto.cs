using psicomedixMonolito.DTOs.Atenciones.Modulos;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Atenciones;

public class AtencionResponseDto
{
    public Guid Id { get; set; }
    public string CodigoAtencion { get; set; } = string.Empty;
    public Guid PacienteId { get; set; }
    public string PacienteNombre { get; set; } = string.Empty;
    public Guid DoctorId { get; set; }
    public string DoctorNombre { get; set; } = string.Empty;
    public Guid ServicioClinicoId { get; set; }
    public string ServicioNombre { get; set; } = string.Empty;
    public Guid? CitaId { get; set; }
    public Guid HistorialClinicoId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaCierre { get; set; }
    public EstadoAtencion Estado { get; set; }
    public decimal CostoFinal { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal SaldoPendiente { get; set; }

    public AnamnesisDto? Anamnesis { get; set; }
    public List<ExamenFisicoDto> ExamenesFisicos { get; set; } = new();
    public List<TactoVaginalDto> TactosVaginales { get; set; } = new();
    public List<EcografiaObstetricaDto> Ecografias { get; set; } = new();
    public ImpresionDiagnosticaDto? ImpresionDiagnostica { get; set; }
}