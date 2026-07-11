namespace psicomedixMonolito.Models.ATENCIONES;

public class EcografiaObstetrica
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    // Biometría (en mm)
    public int? DiametroBiparietal { get; set; } // DBP
    public int? CircunferenciaCefalica { get; set; } // CC
    public int? CircunferenciaAbdominal { get; set; } // CA
    public int? LongitudFemur { get; set; } // LF

    // Otros valores
    public int? PesoFetalEstimado { get; set; } // grs
    public decimal? IndiceLiquidoAmniotico { get; set; } // ILA

    // Placenta y Cordón
    public string? PlacentaLocalizacion { get; set; }
    public string? PlacentaGranum { get; set; } // 0, I, II, III
    public bool CircularCordon { get; set; }
    
    public string? Conclusiones { get; set; }
}