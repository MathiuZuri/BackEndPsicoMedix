namespace psicomedixMonolito.DTOs.Comunes;

public class PaginacionResponseDto<T>
{
    public int Pagina { get; set; }
    public int CantidadPorPagina { get; set; }
    public int TotalRegistros { get; set; }
    public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / CantidadPorPagina);
    public List<T> Datos { get; set; } = new();
}