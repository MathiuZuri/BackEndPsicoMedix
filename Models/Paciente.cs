using System;
using System.Collections.Generic;
using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class Paciente
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CodigoPaciente { get; set; } = string.Empty;
    public string? DNI { get; set; }

    // SECCIÓN 0: CABECERA
    public DateTime? FechaAtencion { get; set; }

    // SECCIÓN I: INFORMANTE
    public bool? TieneInformante { get; set; }
    public string? InformanteNombre { get; set; }
    public string? InformanteParentesco { get; set; }
    public string? InformanteOcupacion { get; set; }
    public EstadoCivil? InformanteEstadoCivil { get; set; }
    public int? InformanteEdad { get; set; }
    public string? InformanteCelular { get; set; }

    // SECCIÓN II: EVALUADO
    public bool? PrimeraAtencionPsicologia { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public GeneroEvaluado? Genero { get; set; }
    public int? EdadAnos { get; set; }
    public int? EdadMeses { get; set; }
    public string? Celular { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public EstadoCivil? EstadoCivil { get; set; }
    public string? LugarNacimiento { get; set; }
    public string? Ocupacion { get; set; }
    public GradoInstruccion? GradoInstruccion { get; set; }
    public string? GradoSemestreCiclo { get; set; }
    public string? Carrera { get; set; }

    // SECCIÓN III: FAMILIARES Y ENTORNO
    public ICollection<PacienteFamiliar> FamiliaresDirectos { get; set; } = new List<PacienteFamiliar>();
    public bool? TieneMasHermanosHijos { get; set; }
    public bool? TieneMediosHermanosPadrastros { get; set; }
    public int? IntegrantesFamilia { get; set; }
    public string? Religion { get; set; }
    public decimal? PesoKg { get; set; }
    public decimal? TallaMetros { get; set; }
    
    // 🚀 Cambiado: Ahora se persiste físicamente en la Base de Datos
    public decimal? IMC { get; set; }

    // SECCIÓN IV: ASIGNACIÓN Y ATENCIÓN (Vinculado directamente al módulo de Doctores)
    public Guid? DoctorId { get; set; }
    public Doctor? Doctor { get; set; }
    
    public string? OtrasEspecialidadesRaw { get; set; } 
    public bool? ConsultaAsuntosLegales { get; set; }
    public string? MotivoConsulta { get; set; }
    
    public Guid? RecepcionadoPorId { get; set; }
    public Usuario? RecepcionadoPor { get; set; }

    // Infraestructura
    public EstadoPaciente Estado { get; set; } = EstadoPaciente.Activo;
    public Guid UsuarioId { get; set; } 
    public Usuario Usuario { get; set; } = null!;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public HistorialClinico? HistorialClinico { get; set; }
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public ICollection<Atencion> Atenciones { get; set; } = new List<Atencion>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
    public ICollection<NotificacionCita> NotificacionesCita { get; set; } = new List<NotificacionCita>();
}