using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class PsicoEvaluacionCognitivaDto
{
    [StringLength(3000, ErrorMessage = "La visión personal (Beck) no debe superar los 3000 caracteres.")]
    public string? BeckPersonal { get; set; }

    [StringLength(3000, ErrorMessage = "La visión del mundo exterior (Beck) no debe superar los 3000 caracteres.")]
    public string? BeckMundoExterior { get; set; }

    [StringLength(3000, ErrorMessage = "La visión del futuro (Beck) no debe superar los 3000 caracteres.")]
    public string? BeckFuturo { get; set; }

    [StringLength(2000, ErrorMessage = "El registro de ideación autolesiva no debe superar los 2000 caracteres.")]
    public string? BeckAutolesiones { get; set; }

    [StringLength(2000, ErrorMessage = "El registro de riesgo de autolisis no debe superar los 2000 caracteres.")]
    public string? BeckAutolisis { get; set; }

    [StringLength(1000, ErrorMessage = "Otras notas de la escala de Beck no deben superar los 1000 caracteres.")]
    public string? BeckOtros { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Pensamiento no debe superar los 3000 caracteres.")]
    public string? FcPensamiento { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Atención no debe superar los 3000 caracteres.")]
    public string? FcAtencion { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Concentración no debe superar los 3000 caracteres.")]
    public string? FcConcentracion { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Lenguaje no debe superar los 3000 caracteres.")]
    public string? FcLenguaje { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Percepción no debe superar los 3000 caracteres.")]
    public string? FcPercepcion { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Juicio no debe superar los 3000 caracteres.")]
    public string? FcJuicio { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Abstracción no debe superar los 3000 caracteres.")]
    public string? FcAbstraccion { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Aprendizaje no debe superar los 3000 caracteres.")]
    public string? FcAprendizaje { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Memoria no debe superar los 3000 caracteres.")]
    public string? FcMemoria { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Motivación no debe superar los 3000 caracteres.")]
    public string? FcMotivacion { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Emoción no debe superar los 3000 caracteres.")]
    public string? FcEmocion { get; set; }

    [StringLength(3000, ErrorMessage = "El análisis de Cálculo no debe superar los 3000 caracteres.")]
    public string? FcCalculo { get; set; }

    [StringLength(3000, ErrorMessage = "Las notas de Coordinación Motora Fina no deben superar los 3000 caracteres.")]
    public string? FcCoordinacionMotoraFina { get; set; }

    [StringLength(3000, ErrorMessage = "Las notas de Coordinación Motora Gruesa no deben superar los 3000 caracteres.")]
    public string? FcCoordinacionMotoraGruesa { get; set; }
}