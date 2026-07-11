namespace psicomedixMonolito.DTOs.Roles;

public class RolResponseDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool EsSistema { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
}