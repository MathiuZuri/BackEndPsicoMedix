using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Horarios;

public class EditarHorarioDoctorDto
{
    [Required(ErrorMessage = "El día de la semana es obligatorio.")]
    public DayOfWeek DiaSemana { get; set; }

    [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
    public TimeOnly HoraInicio { get; set; }

    [Required(ErrorMessage = "La hora de fin es obligatoria.")]
    public TimeOnly HoraFin { get; set; }

    [Required(ErrorMessage = "La fecha de inicio de vigencia es obligatoria.")]
    public DateOnly FechaInicioVigencia { get; set; }

    public DateOnly? FechaFinVigencia { get; set; }

    public bool Activo { get; set; } = true;
}