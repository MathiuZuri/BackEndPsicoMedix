using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class ServicioClinico
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoServicio { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public decimal CostoBase { get; set; }
    public int DuracionMinutos { get; set; }

    public bool RequiereCita { get; set; } = true;
    public bool GeneraHistorial { get; set; } = true;

    public EstadoServicioClinico Estado { get; set; } = EstadoServicioClinico.Activo;

    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public ICollection<Atencion> Atenciones { get; set; } = new List<Atencion>();
    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}