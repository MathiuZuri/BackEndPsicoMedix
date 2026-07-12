using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Pacientes;

public class FamiliarDto
{
    [Required(ErrorMessage = "El parentesco del familiar es obligatorio.")]
    public ParentescoFamiliar? Parentesco { get; set; }

    [Required(ErrorMessage = "El nombre del familiar es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre del familiar no debe superar los 150 caracteres.")]
    public string? Nombres { get; set; }

    [Range(0, 120, ErrorMessage = "La edad del familiar debe estar entre 0 y 120 años.")]
    public int? Edad { get; set; }

    [StringLength(150, ErrorMessage = "La ocupación del familiar no debe superar los 150 caracteres.")]
    public string? Ocupacion { get; set; }

    public bool? VivenJuntos { get; set; }
}