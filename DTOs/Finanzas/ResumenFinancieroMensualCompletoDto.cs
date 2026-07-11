namespace psicomedixMonolito.DTOs.Finanzas;

public class ResumenFinancieroMensualCompletoDto
{
    public int Anio { get; set; }
    public int Mes { get; set; }

    public ResumenCajaDto ResumenCaja { get; set; } = new();
    public ResumenRealAtencionesDto ResumenRealAtenciones { get; set; } = new();

    public List<AjusteFinancieroDto> AjustesFinancieros { get; set; } = new();
}