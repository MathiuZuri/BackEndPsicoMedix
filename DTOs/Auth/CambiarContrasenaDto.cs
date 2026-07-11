using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Auth;

public class CambiarContrasenaDto
{
    [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
    public string ContrasenaActual { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La nueva contraseña debe tener entre 8 y 100 caracteres.")]
    public string ContrasenaNueva { get; set; } = string.Empty;
}