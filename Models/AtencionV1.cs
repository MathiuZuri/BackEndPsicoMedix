using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Models;

public class AtencionV1
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoAtencion { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public Guid ServicioClinicoId { get; set; }
    public ServicioClinico ServicioClinico { get; set; } = null!;

    public Guid? CitaId { get; set; }
    public Cita? Cita { get; set; }

    public Guid HistorialClinicoId { get; set; }
    public HistorialClinico HistorialClinico { get; set; } = null!;

    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
    public DateTime? FechaCierre { get; set; }

    public string MotivoConsulta { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public string? DiagnosticoResumen { get; set; }
    public string? Indicaciones { get; set; }
    public string? Tratamiento { get; set; }

    public EstadoAtencion Estado { get; set; } = EstadoAtencion.Abierta;

    public decimal CostoFinal { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal SaldoPendiente { get; set; }

    public Guid? UsuarioRegistroId { get; set; }
    public Usuario? UsuarioRegistro { get; set; }

    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    public ICollection<HistorialDetalle> HistorialDetalles { get; set; } = new List<HistorialDetalle>();
    
    public ICollection<AjusteFinanciero> AjustesFinancieros { get; set; } = new List<AjusteFinanciero>();
    public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
}