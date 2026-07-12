using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class PsicoAnamnesisHistoriaDto
{
    [StringLength(4000, ErrorMessage = "La nota de sustancias no debe superar los 4000 caracteres.")]
    public string? SustanciasNotasGenerales { get; set; }

    [StringLength(1000, ErrorMessage = "El detalle de sustancias legales no debe superar los 1000 caracteres.")]
    public string? SustanciasLegales { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de consumo de Alcohol (OH) no debe superar los 500 caracteres.")]
    public string? ConsumoOH { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de cigarrillos o vape no debe superar los 500 caracteres.")]
    public string? CigarrillosVape { get; set; }

    [StringLength(1000, ErrorMessage = "El detalle de sustancias no legales no debe superar los 1000 caracteres.")]
    public string? SustanciasNoLegales { get; set; }

    [StringLength(2000, ErrorMessage = "El detalle de medicamentos no debe superar los 2000 caracteres.")]
    public string? Medicamentos { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de suplementos no debe superar los 500 caracteres.")]
    public string? Suplementos { get; set; }

    [StringLength(4000, ErrorMessage = "La nota de enfermedades y accidentes no debe superar los 4000 caracteres.")]
    public string? EnfermedadesAccidentesNotasGenerales { get; set; }

    [StringLength(3000, ErrorMessage = "El campo de enfermedades no debe superar los 3000 caracteres.")]
    public string? Enfermedades { get; set; }

    [StringLength(2000, ErrorMessage = "El campo de accidentes no debe superar los 2000 caracteres.")]
    public string? Accidentes { get; set; }

    [StringLength(2000, ErrorMessage = "El campo de cirugías no debe superar los 2000 caracteres.")]
    public string? Cirugias { get; set; }

    [StringLength(2000, ErrorMessage = "El campo de hospitalizaciones no debe superar los 2000 caracteres.")]
    public string? Hospitalizacion { get; set; }

    [StringLength(3000, ErrorMessage = "Los antecedentes familiares relacionados no deben superar los 3000 caracteres.")]
    public string? FamiliaresAntecedentesRelacionados { get; set; }
}