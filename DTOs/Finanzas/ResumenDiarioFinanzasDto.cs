namespace psicomedixMonolito.DTOs.Finanzas;

public class ResumenDiarioFinanzasDto
{
    public DateOnly Fecha { get; set; }

    public decimal TotalIngresos { get; set; }
    public decimal TotalPendiente { get; set; }
    public decimal TotalDeuda { get; set; }

    public int CantidadPagos { get; set; }
    public int PagosCompletados { get; set; }
    public int PagosParciales { get; set; }
    public int PagosPendientes { get; set; }

    public List<PagoFinanzasDto> Pagos { get; set; } = new();
    
    public int PagosIndividualesCompletados { get; set; }
    public int PagosIndividualesParciales { get; set; }
    public int PagosIndividualesPendientes { get; set; }
}