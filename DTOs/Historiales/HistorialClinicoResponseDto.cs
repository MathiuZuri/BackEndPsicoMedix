using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Historiales;

public class HistorialClinicoResponseDto
{
    public Guid Id { get; set; }
    public string CodigoHistorial { get; set; } = string.Empty;

    public Guid PacienteId { get; set; }
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDni { get; set; } = string.Empty;

    public DateTime FechaApertura { get; set; }
    public EstadoHistorialClinico Estado { get; set; }

    public List<HistorialDetalleResponseDto> Detalles { get; set; } = new();
}