using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Atenciones.Modulos;

public class PsicoEscalasAnimoDto
{
    [Range(0, 10, ErrorMessage = "La escala de irritabilidad debe ser un valor entero del 0 al 10.")]
    public int? EscalaIrritabilidad { get; set; }

    [Range(0, 10, ErrorMessage = "La escala de tristeza debe ser un valor entero del 0 al 10.")]
    public int? EscalaTristeza { get; set; }

    [Range(0, 10, ErrorMessage = "La escala de ansiedad debe ser un valor entero del 0 al 10.")]
    public int? EscalaAnsiedad { get; set; }

    [Range(0, 10, ErrorMessage = "La escala de preocupación debe ser un valor entero del 0 al 10.")]
    public int? EscalaPreocupacion { get; set; }

    [Range(0, 10, ErrorMessage = "La escala de impulsividad debe ser un valor entero del 0 al 10.")]
    public int? EscalaImpulsividad { get; set; }

    [Range(0, 10, ErrorMessage = "La escala de estrés debe ser un valor entero del 0 al 10.")]
    public int? EscalaEstres { get; set; }

    [Range(0, 10, ErrorMessage = "La escala de fatiga debe ser un valor entero del 0 al 10.")]
    public int? EscalaFatiga { get; set; }

    [Range(0, 10, ErrorMessage = "La escala de desmotivación debe ser un valor entero del 0 al 10.")]
    public int? EscalaDesmotivacion { get; set; }
}