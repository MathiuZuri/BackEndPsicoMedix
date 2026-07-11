using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Pacientes;

public class ActualizarContactoPacienteDto
{
    [RegularExpression(@"^\d{9}$", ErrorMessage = "El celular debe tener exactamente 9 dígitos.")]
    public string? Celular { get; set; }

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no debe superar los 150 caracteres.")]
    public string? Correo { get; set; }

    [StringLength(250, ErrorMessage = "La dirección no debe superar los 250 caracteres.")]
    public string? Direccion { get; set; }
}