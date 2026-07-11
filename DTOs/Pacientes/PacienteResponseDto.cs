using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Pacientes;

public class PacienteResponseDto
{
    public Guid Id { get; set; }
    public Guid HistorialClinicoId { get; set; }
    public string CodigoPaciente { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto => $"{Nombres} {Apellidos}";
    public DateTime FechaNacimiento { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public string? Celular { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
    
    // --- DATOS: MÓDULO DE FILIACIÓN ---
    public string? LugarNacimiento { get; set; }
    public string? GradoInstruccion { get; set; }
    public string? Ocupacion { get; set; }
    public string? Religion { get; set; }
    public string? EstadoCivil { get; set; }
    public string? NombrePareja { get; set; }
    public string? CelularPareja { get; set; }

    public EstadoPaciente Estado { get; set; }
    public DateTime FechaRegistro { get; set; }

    public string? CodigoHistorial { get; set; }
}