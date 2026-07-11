using System.ComponentModel.DataAnnotations;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Pagos;

public class CambiarEstadoPagoDto
{
    [Required(ErrorMessage = "El nuevo estado es obligatorio.")]
    public EstadoPago Estado { get; set; }
}