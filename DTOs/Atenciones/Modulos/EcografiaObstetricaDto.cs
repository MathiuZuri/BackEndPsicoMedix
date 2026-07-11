using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class EcografiaObstetricaDto
{
    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    [Range(0, int.MaxValue, ErrorMessage = "El diámetro biparietal debe ser un valor positivo.")]
    public int? DiametroBiparietal { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La circunferencia cefálica debe ser un valor positivo.")]
    public int? CircunferenciaCefalica { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La circunferencia abdominal debe ser un valor positivo.")]
    public int? CircunferenciaAbdominal { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La longitud del fémur debe ser un valor positivo.")]
    public int? LongitudFemur { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El peso fetal estimado debe ser un valor positivo.")]
    public int? PesoFetalEstimado { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El índice de líquido amniótico debe ser positivo.")]
    public decimal? IndiceLiquidoAmniotico { get; set; }

    [StringLength(100, ErrorMessage = "La localización de la placenta no debe superar los 100 caracteres.")]
    public string? PlacentaLocalizacion { get; set; }

    [StringLength(20, ErrorMessage = "El grado de madurez de la placenta (Granum) no debe superar los 20 caracteres.")]
    public string? PlacentaGranum { get; set; }

    public bool CircularCordon { get; set; }

    [StringLength(1000, ErrorMessage = "Las conclusiones no deben superar los 1000 caracteres.")]
    public string? Conclusiones { get; set; }
}