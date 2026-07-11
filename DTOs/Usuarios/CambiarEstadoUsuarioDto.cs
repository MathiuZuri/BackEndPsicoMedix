using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Usuarios;

public class CambiarEstadoUsuarioDto
{
    [Required(ErrorMessage = "El nuevo estado es obligatorio.")]
    public EstadoUsuario Estado { get; set; }
}