using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Models.ATENCIONES;

public class Atencion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CodigoAtencion { get; set; } = string.Empty;

    // ==========================================
    // RELACIONES ADMINISTRATIVAS (CORE)
    // ==========================================
    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;
    
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
    
    public Guid? HistorialClinicoId { get; set; }
    public HistorialClinico? HistorialClinico { get; set; }
    
    public Guid ServicioClinicoId { get; set; }
    public ServicioClinico ServicioClinico { get; set; } = null!;
    
    public Guid? CitaId { get; set; }
    public Cita? Cita { get; set; }

    // ==========================================
    // DATOS DE FLUJO Y TIEMPO
    // ==========================================
    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
    public DateTime? FechaCierre { get; set; }
    public EstadoAtencion Estado { get; set; }

    // ==========================================
    // RELACIONES FINANCIERAS
    // ==========================================
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();

    // ==========================================
    // MÓDULOS CLÍNICOS INDEPENDIENTES
    // ==========================================
    public Anamnesis? Anamnesis { get; set; }
    public ImpresionDiagnostica? ImpresionDiagnostica { get; set; }
    public ICollection<ExamenFisico> ExamenesFisicos { get; set; } = new List<ExamenFisico>();
    public ICollection<TactoVaginal> TactosVaginales { get; set; } = new List<TactoVaginal>();
    public ICollection<EcografiaObstetrica> Ecografias { get; set; } = new List<EcografiaObstetrica>();
}