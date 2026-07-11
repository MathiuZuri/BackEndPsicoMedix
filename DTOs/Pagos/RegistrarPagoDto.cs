using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Pagos;

public class RegistrarPagoDto
{
    [Required(ErrorMessage = "El paciente es obligatorio.")]
    public Guid PacienteId { get; set; }

    [Required(ErrorMessage = "El servicio clínico es obligatorio.")]
    public Guid ServicioClinicoId { get; set; }

    public Guid? CitaId { get; set; }

    public Guid? AtencionId { get; set; }

    [Range(0.01, 999999.99, ErrorMessage = "El monto total debe ser mayor a 0.")]
    public decimal MontoTotal { get; set; }

    [Range(0, 999999.99, ErrorMessage = "El monto pagado no puede ser negativo.")]
    public decimal MontoPagado { get; set; }

    [Range(0, 999999.99, ErrorMessage = "El monto de adelanto no puede ser negativo.")]
    public decimal MontoAdelanto { get; set; }

    [Required(ErrorMessage = "El método de pago es obligatorio.")]
    public MetodoPago MetodoPago { get; set; }

    [StringLength(500, ErrorMessage = "La observación no debe superar los 500 caracteres.")]
    public string? Observacion { get; set; }
}