using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES.AtencionBase;

namespace psicomedixMonolito.Models.ATENCIONES;

public class Atencion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CodigoAtencion { get; set; } = string.Empty;

    // Relaciones Core
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

    // Tiempos y Flujo
    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
    public DateTime? @FechaCierre { get; set; }
    public EstadoAtencion Estado { get; set; }
    public string? ObservacionesIniciales { get; set; }

    // Colecciones Financieras
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();

    // NUEVOS MÓDULOS CLÍNICOS PSICOLÓGICOS SEPARADOS (Todos Opcionales 1:1)
    public PsicoAnamnesisHistoria? AnamnesisHistoria { get; set; }
    public PsicoSomaticoVegetativo? SomaticoVegetativo { get; set; }
    public PsicoEscalasAnimo? EscalasAnimo { get; set; }
    public PsicoDesarrolloPsicosocial? DesarrolloPsicosocial { get; set; }
    public PsicoEvaluacionCognitiva? EvaluacionCognitiva { get; set; }
    public PsicoDiagnosticoCierre? DiagnosticoCierre { get; set; }
}