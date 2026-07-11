namespace psicomedixMonolito.DTOs.Auth;

public class RespuestaInicioSesionDto
{
    public Guid UsuarioId { get; set; }
    public string CodigoUsuario { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Permisos { get; set; } = new();
    public bool DebeCambiarContrasena { get; set; }
}