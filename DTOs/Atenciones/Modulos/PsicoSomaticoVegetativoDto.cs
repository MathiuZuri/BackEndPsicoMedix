using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class PsicoSomaticoVegetativoDto
{
    [StringLength(4000, ErrorMessage = "Las notas de sueño no deben superar los 4000 caracteres.")]
    public string? SuenoNotasGenerales { get; set; }

    [StringLength(50, ErrorMessage = "El momento de inicio de sueño no debe superar los 50 caracteres.")]
    public string? SuenoDuracionInicio { get; set; }

    [StringLength(50, ErrorMessage = "El momento del despertar no debe superar los 50 caracteres.")]
    public string? SuenoDuracionFin { get; set; }

    [StringLength(1000, ErrorMessage = "El detalle de ensoñaciones no debe superar los 1000 caracteres.")]
    public string? Ensonaciones { get; set; }

    [StringLength(1000, ErrorMessage = "El detalle de pesadillas no debe superar los 1000 caracteres.")]
    public string? Pesadillas { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de apnea de sueño no debe superar los 500 caracteres.")]
    public string? ApneaSueno { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de sobresaltos no debe superar los 500 caracteres.")]
    public string? Sobresaltos { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de parálisis de sueño no debe superar los 500 caracteres.")]
    public string? ParalisisSueno { get; set; }

    [StringLength(1000, ErrorMessage = "Otros detalles de sueño no deben superar los 1000 caracteres.")]
    public string? SuenoOtros { get; set; }

    [StringLength(4000, ErrorMessage = "Las notas de alimentación no deben superar los 4000 caracteres.")]
    public string? AlimentacionNotasGenerales { get; set; }

    [StringLength(20, ErrorMessage = "El peso del control no debe superar los 20 caracteres.")]
    public string? Peso { get; set; }

    [StringLength(2000, ErrorMessage = "El aspecto e historial de actividad física no debe superar los 2000 caracteres.")]
    public string? AspectoFisicoActividadFisica { get; set; }

    [StringLength(500, ErrorMessage = "El estado de apetito no debe superar los 500 caracteres.")]
    public string? Apetito { get; set; }

    [StringLength(3000, ErrorMessage = "Los antecedentes de alteraciones clínicas no deben superar los 3000 caracteres.")]
    public string? AntecedentesAlteracionesClinicas { get; set; }

    [StringLength(4000, ErrorMessage = "Las notas de somatización no deben superar los 4000 caracteres.")]
    public string? SomatizacionesNotasGenerales { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de cefaleas no debe superar los 500 caracteres.")]
    public string? Cefalea { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de adormecimientos no debe superar los 500 caracteres.")]
    public string? Adormecimientos { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de sudoraciones no debe superar los 500 caracteres.")]
    public string? Sudoracion { get; set; }

    [StringLength(500, ErrorMessage = "El detalle de rubefacción no debe superar los 500 caracteres.")]
    public string? Rubefaccion { get; set; }

    [StringLength(1000, ErrorMessage = "Otros malestares somáticos no deben superar los 1000 caracteres.")]
    public string? SomatizacionesOtros { get; set; }

    [StringLength(2000, ErrorMessage = "Las notas de signos vitales no deben superar los 2000 caracteres.")]
    public string? SignosVitalesNotasGenerales { get; set; }

    [Range(0.0, 100.0, ErrorMessage = "La saturación de oxígeno (SpO2) debe estar en un rango porcentual entre 0% y 100%.")]
    public decimal? SaturacionOxigeno { get; set; }

    [StringLength(200, ErrorMessage = "La descripción del reflejo pupilar no debe superar los 200 caracteres.")]
    public string? ReflejoPupilar { get; set; }

    [Range(20, 250, ErrorMessage = "La frecuencia cardíaca debe registrarse entre 20 y 250 LPM.")]
    public int? FrecuenciaCardiaca { get; set; }

    [StringLength(1000, ErrorMessage = "Otros signos vitales no deben superar los 1000 caracteres.")]
    public string? SignosVitalesOtros { get; set; }
}