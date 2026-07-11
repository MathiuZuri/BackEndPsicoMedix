using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Models;

public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoUsuario { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public EstadoUsuario Estado { get; set; } = EstadoUsuario.Activo;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoAcceso { get; set; }
    
    public bool DebeCambiarContrasena { get; set; } = false;

    public ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    public ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
    public ICollection<Comprobante> ComprobantesEmitidos { get; set; } = new List<Comprobante>();
    public ICollection<Comprobante> ComprobantesAnulados { get; set; } = new List<Comprobante>();
}