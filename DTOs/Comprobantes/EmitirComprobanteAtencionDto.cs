using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Comprobantes;

public class EmitirComprobanteAtencionDto
{
    [Required(ErrorMessage = "La atención es obligatoria.")]
    public Guid AtencionId { get; set; }

    public TipoComprobante TipoComprobante { get; set; } = TipoComprobante.ResumenAtencion;

    public TipoFormatoImpresion FormatoImpresion { get; set; } = TipoFormatoImpresion.A4;

    [StringLength(500, ErrorMessage = "La observación no debe superar los 500 caracteres.")]
    public string? Observacion { get; set; }
}