namespace psicomedixMonolito.Models;

public class RolPermiso
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid RolId { get; set; }
    public Rol Rol { get; set; } = null!;

    public Guid PermisoId { get; set; }
    public Permiso Permiso { get; set; } = null!;

    public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
}