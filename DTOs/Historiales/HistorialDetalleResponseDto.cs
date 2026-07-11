using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Historiales;

public class HistorialDetalleResponseDto
{
    public Guid Id { get; set; }
    public string CodigoDetalle { get; set; } = string.Empty;

    public Guid HistorialClinicoId { get; set; }

    public TipoMovimientoHistorial TipoMovimiento { get; set; }

    public Guid? CitaId { get; set; }
    public Guid? AtencionId { get; set; }
    public Guid? PagoId { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; }

    public Guid? UsuarioId { get; set; }
    public string? UsuarioNombre { get; set; }
}