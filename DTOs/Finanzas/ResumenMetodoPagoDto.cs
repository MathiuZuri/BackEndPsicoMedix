namespace psicomedixMonolito.DTOs.Finanzas;

public class ResumenMetodoPagoDto
{
    public string MetodoPago { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public int CantidadMovimientos { get; set; }
}