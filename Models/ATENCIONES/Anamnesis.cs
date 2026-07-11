namespace psicomedixMonolito.Models.ATENCIONES;

public class Anamnesis
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Motivo
    public string MotivoConsulta { get; set; } = string.Empty;

    // Fórmula Obstétrica
    public int Gestaciones { get; set; }
    public int HijosVivos { get; set; }
    public int Abortos { get; set; }          // < 22 semanas
    public int PartosPretermino { get; set; } // 22 a < 37 sem
    public int PartosATermino { get; set; }   // 37 - 42 sem

    // Cálculos Gestacionales
    public DateTime? FechaUltimaRegla { get; set; } // FUR
    public DateTime? FechaProbableParto { get; set; } // FPP
    public string? EdadGestacional { get; set; } // Ej. "34 sem y 2 días"

    // Antecedentes Médicos
    public string? Alergias { get; set; }
    public string? EnfermedadesCronicas { get; set; } 
    public string? CirugiasPrevias { get; set; } 
    public string? AntecedentesAdicionales { get; set; }
}