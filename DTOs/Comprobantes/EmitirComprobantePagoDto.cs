using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Comprobantes;

public class EmitirComprobantePagoDto
{
    public Guid PagoId { get; set; }

    [StringLength(50, ErrorMessage = "El código de pago no debe superar los 50 caracteres.")]
    public string? CodigoPago { get; set; }

    public TipoComprobante TipoComprobante { get; set; } = TipoComprobante.BoletaPago;

    public TipoFormatoImpresion FormatoImpresion { get; set; } = TipoFormatoImpresion.A4;

    [StringLength(500, ErrorMessage = "La observación no debe superar los 500 caracteres.")]
    public string? Observacion { get; set; }
}