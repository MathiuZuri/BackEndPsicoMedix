using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Utils.Validations;

namespace psicomedixMonolito.DTOs.Citas;

public class ReprogramarCitaDto
{
    [NotEmptyGuid(ErrorMessage = "El doctor es obligatorio.")]
    public Guid DoctorId { get; set; }

    public Guid? HorarioDoctorId { get; set; }

    [Required(ErrorMessage = "La nueva fecha es obligatoria.")]
    public DateOnly NuevaFecha { get; set; }

    [Required(ErrorMessage = "La nueva hora de inicio es obligatoria.")]
    public TimeOnly NuevaHoraInicio { get; set; }

    [Required(ErrorMessage = "La nueva hora de fin es obligatoria.")]
    public TimeOnly NuevaHoraFin { get; set; }

    [StringLength(500, ErrorMessage = "El motivo de reprogramación no debe superar los 500 caracteres.")]
    public string? MotivoReprogramacion { get; set; }
}