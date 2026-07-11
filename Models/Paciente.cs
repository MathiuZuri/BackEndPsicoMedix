using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class Paciente
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoPaciente { get; set; } = string.Empty;
    public string DNI { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }
    public string Sexo { get; set; } = string.Empty;

    public string? Celular { get; set; }
    public string? Correo { get; set; }
    public string? Direccion { get; set; }

    // --- NUEVOS DATOS: MÓDULO DE FILIACIÓN ---
    public string? LugarNacimiento { get; set; }
    public string? GradoInstruccion { get; set; }
    public string? Ocupacion { get; set; }
    public string? Religion { get; set; }
    public string? EstadoCivil { get; set; }
    public string? NombrePareja { get; set; }
    public string? CelularPareja { get; set; }
    // -----------------------------------------

    public EstadoPaciente Estado { get; set; } = EstadoPaciente.Activo;

    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public HistorialClinico? HistorialClinico { get; set; }

    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public ICollection<Atencion> Atenciones { get; set; } = new List<Atencion>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
    
    // esto es exclusivo de evolution api, no incluir al sistema
    public ICollection<NotificacionCita> NotificacionesCita { get; set; } = new List<NotificacionCita>();
}