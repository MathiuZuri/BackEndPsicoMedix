using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Auth;

public class IniciarSesionDto
{
    [Required(ErrorMessage = "El usuario o correo es obligatorio.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "El usuario o correo debe tener entre 3 y 150 caracteres.")]
    public string UsuarioOCorreo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;
}