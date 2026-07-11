namespace psicomedixMonolito.DTOs.Finanzas;

public class ResumenCajaDto
{
    public decimal TotalIngresos { get; set; }
    public int CantidadMovimientos { get; set; }

    public decimal TotalEfectivo { get; set; }
    public decimal TotalYape { get; set; }
    public decimal TotalPlin { get; set; }
    public decimal TotalTransferencia { get; set; }
    public decimal TotalTarjeta { get; set; }
    public decimal TotalOtro { get; set; }

    public List<ResumenMetodoPagoDto> MetodosPago { get; set; } = new();
    public List<PagoFinanzasDto> Movimientos { get; set; } = new();
}