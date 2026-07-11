using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Servicios;

public class ServicioClinicoResponseDto
{
    public Guid Id { get; set; }
    public string CodigoServicio { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal CostoBase { get; set; }
    public int DuracionMinutos { get; set; }
    public bool RequiereCita { get; set; }
    public bool GeneraHistorial { get; set; }
    public EstadoServicioClinico Estado { get; set; }
}