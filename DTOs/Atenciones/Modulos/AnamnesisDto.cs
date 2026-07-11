using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class AnamnesisDto
{
    [Required(ErrorMessage = "El motivo de consulta es obligatorio.")]
    [StringLength(1500, ErrorMessage = "El motivo de consulta no debe superar los 1500 caracteres.")]
    public string MotivoConsulta { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Las gestaciones no pueden ser negativas.")]
    public int Gestaciones { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Los hijos vivos no pueden ser negativos.")]
    public int HijosVivos { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Los abortos no pueden ser negativos.")]
    public int Abortos { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Los partos pretérmino no pueden ser negativos.")]
    public int PartosPretermino { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Los partos a término no pueden ser negativos.")]
    public int PartosATermino { get; set; }

    public DateTime? FechaUltimaRegla { get; set; }
    public DateTime? FechaProbableParto { get; set; }

    [StringLength(50, ErrorMessage = "La edad gestacional no debe superar los 50 caracteres.")]
    public string? EdadGestacional { get; set; }

    [StringLength(500, ErrorMessage = "Las alergias no deben superar los 500 caracteres.")]
    public string? Alergias { get; set; }

    [StringLength(500, ErrorMessage = "Las enfermedades crónicas no deben superar los 500 caracteres.")]
    public string? EnfermedadesCronicas { get; set; }

    [StringLength(500, ErrorMessage = "Las cirugías previas no deben superar los 500 caracteres.")]
    public string? CirugiasPrevias { get; set; }

    [StringLength(1500, ErrorMessage = "Los antecedentes adicionales no deben superar los 1500 caracteres.")]
    public string? AntecedentesAdicionales { get; set; }
}