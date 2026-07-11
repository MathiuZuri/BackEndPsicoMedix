using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Finanzas;

public class RegistrarAjusteFinancieroDto
{
    [Required(ErrorMessage = "El pago es obligatorio.")]
    public Guid PagoId { get; set; }

    [Required(ErrorMessage = "El tipo de ajuste es obligatorio.")]
    public TipoAjusteFinanciero TipoAjuste { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "El monto del ajuste debe ser mayor a 0.")]
    public decimal MontoAjuste { get; set; }

    [Required(ErrorMessage = "El motivo del ajuste es obligatorio.")]
    [StringLength(500, ErrorMessage = "El motivo no debe superar los 500 caracteres.")]
    public string Motivo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "La observación no debe superar los 500 caracteres.")]
    public string? Observacion { get; set; }
}