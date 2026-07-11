using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class ImpresionDiagnosticaDto
{
    [Required(ErrorMessage = "El diagnóstico principal es obligatorio.")]
    [StringLength(500, ErrorMessage = "El diagnóstico principal no debe superar los 500 caracteres.")]
    public string DiagnosticoPrincipal { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Los diagnósticos secundarios no deben superar los 1000 caracteres.")]
    public string? DiagnosticosSecundarios { get; set; }

    [StringLength(2500, ErrorMessage = "Las indicaciones/receta no deben superar los 2500 caracteres.")]
    public string IndicacionesReceta { get; set; } = string.Empty;

    public DateTime? FechaProximaCita { get; set; }

    [StringLength(250, ErrorMessage = "El motivo de la próxima cita no debe superar los 250 caracteres.")]
    public string? MotivoProximaCita { get; set; }
}