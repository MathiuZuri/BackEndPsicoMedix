namespace psicomedixMonolito.DTOs.Comprobantes;

public class ComprobanteDetalleDto
{
    public Guid Id { get; set; }

    public string CodigoServicio { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;

    public int Cantidad { get; set; } = 1;

    public decimal PrecioUnitarioFinal { get; set; }

    public decimal Subtotal { get; set; }
    public decimal TasaImpuesto { get; set; }
    public decimal MontoImpuesto { get; set; }
    public decimal Total { get; set; }
}