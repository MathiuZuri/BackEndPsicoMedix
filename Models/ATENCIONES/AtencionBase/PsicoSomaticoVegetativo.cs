namespace psicomedixMonolito.Models.ATENCIONES.AtencionBase;

public class PsicoSomaticoVegetativo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    // Sueño
    public string? SuenoNotasGenerales { get; set; }
    public string? SuenoDuracionInicio { get; set; }
    public string? SuenoDuracionFin { get; set; }
    public string? Ensonaciones { get; set; }
    public string? Pesadillas { get; set; }
    public string? ApneaSueno { get; set; }
    public string? Sobresaltos { get; set; }
    public string? ParalisisSueno { get; set; }
    public string? SuenoOtros { get; set; }

    // Alimentación
    public string? AlimentacionNotasGenerales { get; set; }
    public string? Peso { get; set; }
    public string? AspectoFisicoActividadFisica { get; set; }
    public string? Apetito { get; set; }
    public string? AntecedentesAlteracionesClinicas { get; set; }

    // Somatizaciones
    public string? SomatizacionesNotasGenerales { get; set; }
    public string? Cefalea { get; set; }
    public string? Adormecimientos { get; set; }
    public string? Sudoracion { get; set; }
    public string? Rubefaccion { get; set; }
    public string? SomatizacionesOtros { get; set; }

    // Signos Vitales
    public string? SignosVitalesNotasGenerales { get; set; }
    public decimal? SaturacionOxigeno { get; set; }
    public string? ReflejoPupilar { get; set; }
    public int? FrecuenciaCardiaca { get; set; }
    public string? SignosVitalesOtros { get; set; }
}