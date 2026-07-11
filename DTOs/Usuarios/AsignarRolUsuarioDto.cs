using psicomedixMonolito.Utils.Validations;

namespace psicomedixMonolito.DTOs.Usuarios;

public class AsignarRolUsuarioDto
{
    [NotEmptyGuid(ErrorMessage = "El usuario es obligatorio.")]
    public Guid UsuarioId { get; set; }

    [NotEmptyGuid(ErrorMessage = "El rol es obligatorio.")]
    public Guid RolId { get; set; }
}