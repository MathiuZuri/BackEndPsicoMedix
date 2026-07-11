namespace psicomedixMonolito.Models.ATENCIONES;

public class ImpresionDiagnostica
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    public string DiagnosticoPrincipal { get; set; } = string.Empty;
    public string? DiagnosticosSecundarios { get; set; }
    
    // Este campo guardará la receta en texto libre o un JSON con los medicamentos
    public string IndicacionesReceta { get; set; } = string.Empty; 

    public DateTime? FechaProximaCita { get; set; }
    public string? MotivoProximaCita { get; set; }
}