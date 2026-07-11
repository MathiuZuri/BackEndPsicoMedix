using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Citas;

public class CitaResponseDto
{
    public Guid Id { get; set; }
    public string CodigoCita { get; set; } = string.Empty;
    public Guid PacienteId { get; set; }
    public string PacienteNombre { get; set; } = string.Empty;
    public Guid DoctorId { get; set; }
    public string DoctorNombre { get; set; } = string.Empty;
    public Guid ServicioClinicoId { get; set; }
    public string ServicioNombre { get; set; } = string.Empty;
    public DateOnly Fecha { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public EstadoCita Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
}