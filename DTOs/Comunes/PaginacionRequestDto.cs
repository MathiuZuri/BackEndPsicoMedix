using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.Comunes;

public class PaginacionRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "La página debe ser mayor o igual a 1.")]
    public int Pagina { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "La cantidad por página debe estar entre 1 y 100.")]
    public int CantidadPorPagina { get; set; } = 20;

    public string? OrdenarPor { get; set; } = "FechaPago";

    public bool OrdenDescendente { get; set; } = true;
}