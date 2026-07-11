namespace psicomedixMonolito.DTOs.Horarios;

public class HorarioDoctorResponseDto
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public string DoctorNombre { get; set; } = string.Empty;
    public DayOfWeek DiaSemana { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public DateOnly FechaInicioVigencia { get; set; }
    public DateOnly? FechaFinVigencia { get; set; }
    public bool Activo { get; set; }
}