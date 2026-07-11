using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class HistorialDetalle
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoDetalle { get; set; } = string.Empty;

    public Guid HistorialClinicoId { get; set; }
    public HistorialClinico HistorialClinico { get; set; } = null!;

    public TipoMovimientoHistorial TipoMovimiento { get; set; }

    public Guid? CitaId { get; set; }
    public Cita? Cita { get; set; }

    public Guid? AtencionId { get; set; }
    public Atencion? Atencion { get; set; }

    public Guid? PagoId { get; set; }
    public Pago? Pago { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public Guid? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}