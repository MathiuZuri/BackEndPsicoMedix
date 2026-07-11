namespace psicomedixMonolito.DTOs.Horarios;

public class MatrizSemanalDto
{
    public Guid DoctorId { get; set; }
    public string DoctorNombre { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public DateOnly FechaInicioSemana { get; set; }
    public DateOnly FechaFinSemana { get; set; }
    public List<FilaMatrizDto> Filas { get; set; } = new();
}

public class FilaMatrizDto
{
    public string RangoHora { get; set; } = string.Empty; // Ej: "08:00 - 08:30"
    public TimeOnly HoraInicio { get; set; }
    public List<CeldaMatrizDto> CeldasDias { get; set; } = new(); 
}

public class CeldaMatrizDto
{
    public DayOfWeek DiaSemana { get; set; }
    public DateOnly FechaCelda { get; set; }
    public string Estado { get; set; } = "FueraHorario"; // "FueraHorario", "Disponible", "Ocupado"
    
    public Guid? CitaId { get; set; }
    public string? PacienteNombre { get; set; }
    public string? ServicioNombre { get; set; }
    public string? CodigoCita { get; set; }
}