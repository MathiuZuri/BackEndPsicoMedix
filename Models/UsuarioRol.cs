namespace psicomedixMonolito.Models;

public class UsuarioRol
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public Guid RolId { get; set; }
    public Rol Rol { get; set; } = null!;

    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
    public bool Activo { get; set; } = true;
}