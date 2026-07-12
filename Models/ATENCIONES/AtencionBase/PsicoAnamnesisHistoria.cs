namespace psicomedixMonolito.Models.ATENCIONES.AtencionBase;

public class PsicoAnamnesisHistoria
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    // Sustancias
    public string? SustanciasNotasGenerales { get; set; }
    public string? SustanciasLegales { get; set; }
    public string? ConsumoOH { get; set; }
    public string? CigarrillosVape { get; set; }
    public string? SustanciasNoLegales { get; set; }
    public string? Medicamentos { get; set; }
    public string? Suplementos { get; set; }

    // Enfermedades y Accidentes
    public string? EnfermedadesAccidentesNotasGenerales { get; set; }
    public string? Enfermedades { get; set; }
    public string? Accidentes { get; set; }
    public string? Cirugias { get; set; }
    public string? Hospitalizacion { get; set; }
    public string? FamiliaresAntecedentesRelacionados { get; set; }
}