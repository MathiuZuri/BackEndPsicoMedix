using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Citas;

public class CrearCitaDto
{
    [Required(ErrorMessage = "El paciente es obligatorio.")]
    public Guid PacienteId { get; set; }

    [Required(ErrorMessage = "El doctor es obligatorio.")]
    public Guid DoctorId { get; set; }

    [Required(ErrorMessage = "El servicio clínico es obligatorio.")]
    public Guid ServicioClinicoId { get; set; }

    public Guid? HorarioDoctorId { get; set; }

    [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
    public DateOnly Fecha { get; set; }

    [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
    public TimeOnly HoraInicio { get; set; }

    [Required(ErrorMessage = "La hora de fin es obligatoria.")]
    public TimeOnly HoraFin { get; set; }

    [Required(ErrorMessage = "El motivo de la cita es obligatorio.")]
    [StringLength(300, MinimumLength = 3, ErrorMessage = "El motivo debe tener entre 3 y 300 caracteres.")]
    public string Motivo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Las observaciones no deben superar los 500 caracteres.")]
    public string? Observaciones { get; set; }
}