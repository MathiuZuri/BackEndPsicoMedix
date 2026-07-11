using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Models;

public class HistorialClinico
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoHistorial { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public DateTime FechaApertura { get; set; } = DateTime.UtcNow;

    public EstadoHistorialClinico Estado { get; set; } = EstadoHistorialClinico.Activo;

    public ICollection<HistorialDetalle> Detalles { get; set; } = new List<HistorialDetalle>();
    public ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
}