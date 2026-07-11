using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Doctores;

public class DoctorResponseDto
{
    public Guid Id { get; set; }
    public string CodigoDoctor { get; set; } = string.Empty;
    public string CMP { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombres} {Apellidos}";
    public string Especialidad { get; set; } = string.Empty;
    public string? Celular { get; set; }
    public string? Correo { get; set; }
    public DateTime FechaInicioContrato { get; set; }
    public DateTime? FechaFinContrato { get; set; }
    public EstadoDoctor Estado { get; set; }
}