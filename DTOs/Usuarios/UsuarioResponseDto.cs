using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Usuarios;

public class UsuarioResponseDto
{
    public Guid Id { get; set; }
    public string CodigoUsuario { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombres} {Apellidos}";
    public string UserName { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public EstadoUsuario Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? UltimoAcceso { get; set; }
}