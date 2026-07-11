namespace psicomedixMonolito.DTOs.Finanzas;

public class ResumenAnualFinanzasDto
{
    public int Anio { get; set; }

    public decimal TotalIngresos { get; set; }
    public decimal TotalPendiente { get; set; }
    public decimal TotalDeuda { get; set; }

    public int CantidadPagos { get; set; }
    public int PagosCompletados { get; set; }
    public int PagosParciales { get; set; }
    public int PagosPendientes { get; set; }

    public List<ResumenMensualFinanzasDto> Meses { get; set; } = new();
}