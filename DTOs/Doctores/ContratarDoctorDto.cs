using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Doctores;

public class ContratarDoctorDto
{
    // ---- Datos del Doctor ----
    [Required(ErrorMessage = "El CMP es obligatorio.")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "El CMP debe tener entre 3 y 20 caracteres.")]
    public string CMP { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres.")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "La especialidad debe tener entre 3 y 120 caracteres.")]
    public string Especialidad { get; set; } = string.Empty;

    [RegularExpression(@"^\d{9}$", ErrorMessage = "El celular debe tener exactamente 9 dígitos.")]
    public string? Celular { get; set; }

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no debe superar los 150 caracteres.")]
    public string? Correo { get; set; }

    [Required(ErrorMessage = "La fecha de inicio de contrato es obligatoria.")]
    public DateTime FechaInicioContrato { get; set; }

    public DateTime? FechaFinContrato { get; set; }

    // ---- Datos del Usuario ----
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo del usuario es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no debe superar los 150 caracteres.")]
    public string CorreoUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
    public string Password { get; set; } = string.Empty;

    public Guid? RolId { get; set; }

    public List<Guid>? PermisosIds { get; set; }
}