using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Utils.Validations;

namespace psicomedixMonolito.DTOs.Horarios;

public class CrearHorarioDoctorDto
{
    [NotEmptyGuid(ErrorMessage = "El doctor es obligatorio.")]
    public Guid DoctorId { get; set; }

    [Required(ErrorMessage = "El día de la semana es obligatorio.")]
    public DayOfWeek DiaSemana { get; set; }

    [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
    public TimeOnly HoraInicio { get; set; }

    [Required(ErrorMessage = "La hora de fin es obligatoria.")]
    public TimeOnly HoraFin { get; set; }

    [Required(ErrorMessage = "La fecha de inicio de vigencia es obligatoria.")]
    public DateOnly FechaInicioVigencia { get; set; }

    public DateOnly? FechaFinVigencia { get; set; }
}