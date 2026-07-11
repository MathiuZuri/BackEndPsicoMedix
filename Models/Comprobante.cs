using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class Comprobante
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoComprobante { get; set; } = string.Empty;

    public string Serie { get; set; } = string.Empty;
    public int Numero { get; set; }

    public TipoComprobante TipoComprobante { get; set; }
    public EstadoComprobante Estado { get; set; } = EstadoComprobante.Emitido;
    public TipoFormatoImpresion FormatoImpresion { get; set; } = TipoFormatoImpresion.A4;

    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public Guid? PagoId { get; set; }
    public Pago? Pago { get; set; }

    public Guid? CitaId { get; set; }
    public Cita? Cita { get; set; }

    public Guid? AtencionId { get; set; }
    public Atencion? Atencion { get; set; }

    public Guid? HistorialClinicoId { get; set; }
    public HistorialClinico? HistorialClinico { get; set; }

    public TipoDocumentoComprobante TipoDocumentoPaciente { get; set; } = TipoDocumentoComprobante.DNI;
    public string NumeroDocumentoPaciente { get; set; } = string.Empty;
    public string NombrePaciente { get; set; } = string.Empty;
    public string? DireccionPaciente { get; set; }

    public decimal Subtotal { get; set; }
    public decimal TasaImpuesto { get; set; }
    public decimal MontoImpuesto { get; set; }
    public decimal Total { get; set; }

    public DateTime FechaEmision { get; set; } = DateTime.UtcNow;

    public Guid UsuarioEmisionId { get; set; }
    public Usuario UsuarioEmision { get; set; } = null!;

    public DateTime? FechaAnulacion { get; set; }
    public Guid? UsuarioAnulacionId { get; set; }
    public Usuario? UsuarioAnulacion { get; set; }
    public string? MotivoAnulacion { get; set; }

    public string? Observacion { get; set; }

    public string DatosSnapshotJson { get; set; } = "{}";

    public ICollection<ComprobanteDetalle> Detalles { get; set; } = new List<ComprobanteDetalle>();
}