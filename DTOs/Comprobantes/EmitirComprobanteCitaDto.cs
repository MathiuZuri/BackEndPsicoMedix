using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Comprobantes;

public class EmitirComprobanteCitaDto
{
    [Required(ErrorMessage = "La cita es obligatoria.")]
    public Guid CitaId { get; set; }

    public TipoComprobante TipoComprobante { get; set; } = TipoComprobante.ConstanciaCita;

    public TipoFormatoImpresion FormatoImpresion { get; set; } = TipoFormatoImpresion.A4;

    [StringLength(500, ErrorMessage = "La observación no debe superar los 500 caracteres.")]
    public string? Observacion { get; set; }
}