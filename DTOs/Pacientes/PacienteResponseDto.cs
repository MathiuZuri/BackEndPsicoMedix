using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Pacientes;

public class PacienteResponseDto
{
    public Guid Id { get; set; }
    public string CodigoPaciente { get; set; } = string.Empty;
    public string? DNI { get; set; }
    
    // SECCIÓN 0: CABECERA
    public DateTime? FechaAtencion { get; set; }

    // SECCIÓN I: INFORMANTE
    public bool? TieneInformante { get; set; }
    public string? InformanteNombre { get; set; }
    public string? InformanteParentesco { get; set; }
    public string? InformanteOcupacion { get; set; }
    public string? InformanteEstadoCivil { get; set; }
    public int? InformanteEdad { get; set; }
    public string? InformanteCelular { get; set; }

    // SECCIÓN II: EVALUADO
    public bool? PrimeraAtencionPsicologia { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? NombreCompleto { get; set; } // Nombre extendido expuesto
    public string? Genero { get; set; }
    public int? EdadAnos { get; set; }
    public int? EdadMeses { get; set; }
    public string? Celular { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? EstadoCivil { get; set; }
    public string? LugarNacimiento { get; set; } // O string? según tu preferencia
    public string? Ocupacion { get; set; }
    public string? GradoInstruccion { get; set; }
    public string? GradoSemestreCiclo { get; set; }
    public string? Carrera { get; set; }

    // SECCIÓN III: FAMILIARES Y ENTORNO
    public List<FamiliarDto> FamiliaresDirectos { get; set; } = new();
    public bool? TieneMasHermanosHijos { get; set; }
    public bool? TieneMediosHermanosPadrastros { get; set; }
    public int? IntegrantesFamilia { get; set; }
    public string? Religion { get; set; }
    public decimal? PesoKg { get; set; }
    public decimal? TallaMetros { get; set; }
    public decimal? IMC { get; set; }

    // SECCIÓN IV: ASIGNACIÓN Y ATENCIÓN
    public Guid? DoctorId { get; set; }
    public List<string> OtrasEspecialidadesRequeridas { get; set; } = new();
    public bool? ConsultaAsuntosLegales { get; set; }
    public string? MotivoConsulta { get; set; }
    public Guid? RecepcionadoPorId { get; set; }

    // Infraestructura
    public string? Estado { get; set; }
    public string? CodigoHistorial { get; set; }
    public DateTime FechaRegistro { get; set; }
    
    // Datos extra de las fk
    
    public string? DoctorNombre { get; set; }
    public string? RecepcionadoPorNombre { get; set; }
}