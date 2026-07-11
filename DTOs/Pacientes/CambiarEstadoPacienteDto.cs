using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Pacientes;

public class CambiarEstadoPacienteDto
{
    [Required(ErrorMessage = "El nuevo estado es obligatorio.")]
    public EstadoPaciente Estado { get; set; }
}