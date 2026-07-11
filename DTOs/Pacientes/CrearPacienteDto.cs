using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Pacientes;

public class CrearPacienteDto
{
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [RegularExpression(@"^\d{8,11}$", ErrorMessage = "El DNI debe tener entre 8 y 11 dígitos numéricos.")]
    public string DNI { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 100 caracteres.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 100 caracteres.")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    [RegularExpression(@"^[MF]$", ErrorMessage = "El sexo debe ser 'M' o 'F'.")]
    public string Sexo { get; set; } = string.Empty;

    [RegularExpression(@"^\d{9}$", ErrorMessage = "El celular debe tener exactamente 9 dígitos.")]
    public string? Celular { get; set; }

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no debe superar los 150 caracteres.")]
    public string? Correo { get; set; }

    [StringLength(250, ErrorMessage = "La dirección no debe superar los 250 caracteres.")]
    public string? Direccion { get; set; }

    // --- Módulo de filiación ---
    [StringLength(150, ErrorMessage = "El lugar de nacimiento no debe superar los 150 caracteres.")]
    public string? LugarNacimiento { get; set; }

    [StringLength(100, ErrorMessage = "El grado de instrucción no debe superar los 100 caracteres.")]
    public string? GradoInstruccion { get; set; }

    [StringLength(150, ErrorMessage = "La ocupación no debe superar los 150 caracteres.")]
    public string? Ocupacion { get; set; }

    [StringLength(50, ErrorMessage = "La religión no debe superar los 50 caracteres.")]
    public string? Religion { get; set; }

    [StringLength(50, ErrorMessage = "El estado civil no debe superar los 50 caracteres.")]
    public string? EstadoCivil { get; set; }

    [StringLength(150, ErrorMessage = "El nombre de la pareja no debe superar los 150 caracteres.")]
    public string? NombrePareja { get; set; }

    [RegularExpression(@"^\d{9}$", ErrorMessage = "El celular de la pareja debe tener exactamente 9 dígitos.")]
    public string? CelularPareja { get; set; }
}