using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class TactoVaginalDto
{
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    [Range(0, int.MaxValue, ErrorMessage = "La dilatación debe ser un valor positivo.")]
    public int? Dilatacion { get; set; }

    [Range(0, 100, ErrorMessage = "El borramiento debe estar entre 0 y 100.")]
    public int? Borramiento { get; set; }

    [StringLength(50, ErrorMessage = "La altura de presentación no debe superar los 50 caracteres.")]
    public string? AlturaPresentacion { get; set; }

    [StringLength(100, ErrorMessage = "Las membranas ovulares no deben superar los 100 caracteres.")]
    public string? MembranasOvulares { get; set; }

    [StringLength(50, ErrorMessage = "El color del líquido no debe superar los 50 caracteres.")]
    public string? ColorLiquido { get; set; }

    [StringLength(50, ErrorMessage = "La pelvis no debe superar los 50 caracteres.")]
    public string? Pelvis { get; set; }

    [StringLength(100, ErrorMessage = "La variedad de presentación no debe superar los 100 caracteres.")]
    public string? VariedadPresentacion { get; set; }
}