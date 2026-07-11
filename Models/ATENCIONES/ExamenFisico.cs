namespace psicomedixMonolito.Models.ATENCIONES;

public class ExamenFisico
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    public DateTime FechaHoraExamen { get; set; } = DateTime.UtcNow; // Crucial si hay varios

    // Apreciación General
    public bool Lotep { get; set; } // Lúcida, orientada
    public string? EstadoGeneral { get; set; } 
    public string? EstadoHidratacion { get; set; }
    public string? EstadoNutricion { get; set; }
    public int? EscalaGlasgow { get; set; } // Ej. 15

    // Abdomen
    public bool UteroGravido { get; set; }
    public int? AlturaUterina { get; set; } // cm
    public string? SituacionPosicionPresentacion { get; set; } 
    public int? LatidosCardiacosFetales { get; set; } // lpm
    public string? MovimientosFetales { get; set; } // Ausentes, +, ++, +++
    public string? TonoUterino { get; set; } // Conservado, Aumentado
    public string? DinamicaUterina { get; set; } // Ausente, Esporádica, etc.

    // Genitourinario
    public bool SangradoTv { get; set; }
    public bool PerdidaLiquidoAmniotico { get; set; }
    public string? ColorLiquidoAmniotico { get; set; } // Verde, Claro (Si aplica)
    public bool TaponMucoso { get; set; }
    public bool FlujoVaginal { get; set; }
    public string? PunoPercusionLumbar { get; set; } // PPL (+ / -)

    // Extremidades
    public string? Edemas { get; set; } // No, +, ++, +++
    public string? ReflejosOsteotendinosos { get; set; } // ++, +++
}