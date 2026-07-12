using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class PsicoDiagnosticoCierreDto
{
    [StringLength(2000, ErrorMessage = "La caja de diagnóstico diferencial izquierdo no debe superar los 2000 caracteres.")]
    public string? DiagnosticoDiferencial1 { get; set; }

    [StringLength(2000, ErrorMessage = "La caja de diagnóstico diferencial central no debe superar los 2000 caracteres.")]
    public string? DiagnosticoDiferencial2 { get; set; }

    [StringLength(2000, ErrorMessage = "La caja de diagnóstico diferencial derecho no debe superar los 2000 caracteres.")]
    public string? DiagnosticoDiferencial3 { get; set; }

    [StringLength(4000, ErrorMessage = "La conclusión de la impresión diagnóstica no debe superar los 4000 caracteres.")]
    public string? ImpresionDiagnostica { get; set; }

    [StringLength(4000, ErrorMessage = "Las recomendaciones terapéuticas no deben superar los 4000 caracteres.")]
    public string? Recomendaciones { get; set; }
}