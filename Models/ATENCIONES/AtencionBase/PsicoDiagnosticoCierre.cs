namespace psicomedixMonolito.Models.ATENCIONES.AtencionBase;

public class PsicoDiagnosticoCierre
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    public string? DiagnosticoDiferencial1 { get; set; }
    public string? DiagnosticoDiferencial2 { get; set; }
    public string? DiagnosticoDiferencial3 { get; set; }
    public string? ImpresionDiagnostica { get; set; }
    public string? Recomendaciones { get; set; }
}