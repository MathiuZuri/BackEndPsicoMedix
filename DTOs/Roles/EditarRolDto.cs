using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Roles;

public class EditarRolDto
{
    [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre del rol debe tener entre 3 y 80 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "La descripción no debe superar los 250 caracteres.")]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;
}