using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class Pago
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoPago { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public Guid ServicioClinicoId { get; set; }
    public ServicioClinico ServicioClinico { get; set; } = null!;

    public Guid? CitaId { get; set; }
    public Cita? Cita { get; set; }

    public Guid? AtencionId { get; set; }
    public Atencion? Atencion { get; set; }

    public decimal MontoTotal { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal SaldoPendiente { get; set; }

    public decimal MontoAdelanto { get; set; }

    public MetodoPago MetodoPago { get; set; }
    public EstadoPago Estado { get; set; } = EstadoPago.Pendiente;

    public string? Observacion { get; set; }

    public DateTime FechaPago { get; set; } = DateTime.UtcNow;

    public Guid? UsuarioRegistroId { get; set; }
    public Usuario? UsuarioRegistro { get; set; }

    public ICollection<HistorialDetalle> HistorialDetalles { get; set; } = new List<HistorialDetalle>();
    
    public ICollection<AjusteFinanciero> AjustesFinancieros { get; set; } = new List<AjusteFinanciero>();
    public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
}