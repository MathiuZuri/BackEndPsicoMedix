namespace psicomedixMonolito.DTOs.Finanzas;

public class ResumenRealAtencionesDto
{
    public decimal TotalFacturadoReal { get; set; }
    public decimal TotalPagadoReal { get; set; }
    public decimal TotalDeudaReal { get; set; }
    public decimal TotalSobrepagos { get; set; }

    public int AtencionesPagadas { get; set; }
    public int AtencionesParciales { get; set; }
    public int AtencionesPendientes { get; set; }
    public int AtencionesSobrepagadas { get; set; }

    public List<EstadoPagoAtencionDto> EstadosAtenciones { get; set; } = new();
}