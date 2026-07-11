using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class ExamenFisicoDto
{
    public DateTime FechaHoraExamen { get; set; } = DateTime.UtcNow;

    public bool Lotep { get; set; }

    [StringLength(50, ErrorMessage = "El estado general no debe superar los 50 caracteres.")]
    public string? EstadoGeneral { get; set; }

    [StringLength(50, ErrorMessage = "El estado de hidratación no debe superar los 50 caracteres.")]
    public string? EstadoHidratacion { get; set; }

    [StringLength(50, ErrorMessage = "El estado nutricional no debe superar los 50 caracteres.")]
    public string? EstadoNutricion { get; set; }

    [Range(3, 15, ErrorMessage = "La escala de Glasgow debe estar entre 3 y 15.")]
    public int? EscalaGlasgow { get; set; }

    public bool UteroGravido { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La altura uterina debe ser un valor positivo.")]
    public int? AlturaUterina { get; set; }

    [StringLength(100, ErrorMessage = "La situación/posición/presentación no debe superar los 100 caracteres.")]
    public string? SituacionPosicionPresentacion { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Los latidos cardiacos fetales deben ser un valor positivo.")]
    public int? LatidosCardiacosFetales { get; set; }

    [StringLength(50, ErrorMessage = "Los movimientos fetales no deben superar los 50 caracteres.")]
    public string? MovimientosFetales { get; set; }

    [StringLength(50, ErrorMessage = "El tono uterino no debe superar los 50 caracteres.")]
    public string? TonoUterino { get; set; }

    [StringLength(100, ErrorMessage = "La dinámica uterina no debe superar los 100 caracteres.")]
    public string? DinamicaUterina { get; set; }

    public bool SangradoTv { get; set; }
    public bool PerdidaLiquidoAmniotico { get; set; }

    [StringLength(50, ErrorMessage = "El color del líquido amniótico no debe superar los 50 caracteres.")]
    public string? ColorLiquidoAmniotico { get; set; }

    public bool TaponMucoso { get; set; }
    public bool FlujoVaginal { get; set; }

    [StringLength(50, ErrorMessage = "La puño percusión lumbar no debe superar los 50 caracteres.")]
    public string? PunoPercusionLumbar { get; set; }

    [StringLength(50, ErrorMessage = "Los edemas no deben superar los 50 caracteres.")]
    public string? Edemas { get; set; }

    [StringLength(50, ErrorMessage = "Los reflejos osteotendinosos no deben superar los 50 caracteres.")]
    public string? ReflejosOsteotendinosos { get; set; }
}