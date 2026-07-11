using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Comprobantes;

public class EmitirComprobanteEstadoCuentaDto
{
    [Required(ErrorMessage = "El paciente es obligatorio.")]
    public Guid PacienteId { get; set; }

    public TipoComprobante TipoComprobante { get; set; } = TipoComprobante.EstadoCuenta;

    public TipoFormatoImpresion FormatoImpresion { get; set; } = TipoFormatoImpresion.A4;

    [StringLength(500, ErrorMessage = "La observación no debe superar los 500 caracteres.")]
    public string? Observacion { get; set; }
}