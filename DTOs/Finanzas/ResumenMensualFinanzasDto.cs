namespace psicomedixMonolito.DTOs.Finanzas;

public class ResumenMensualFinanzasDto
{
    public int Anio { get; set; }
    public int Mes { get; set; }

    public decimal TotalIngresos { get; set; }
    public decimal TotalPendiente { get; set; }
    public decimal TotalDeuda { get; set; }

    public int CantidadPagos { get; set; }
    public int PagosCompletados { get; set; }
    public int PagosParciales { get; set; }
    public int PagosPendientes { get; set; }

    public List<ResumenDiarioFinanzasDto> Dias { get; set; } = new();
}