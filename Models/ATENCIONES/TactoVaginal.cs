namespace psicomedixMonolito.Models.ATENCIONES;

public class TactoVaginal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Lo podemos atar a la Atención directamente para simplificar consultas
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    public DateTime FechaHora { get; set; } = DateTime.UtcNow;

    public int? Dilatacion { get; set; } // cm
    public int? Borramiento { get; set; } // %
    public string? AlturaPresentacion { get; set; } // -3, -2, 0, +1, etc.
    public string? MembranasOvulares { get; set; } // Integras, rotas, etc.
    public string? ColorLiquido { get; set; } // Si están rotas
    public string? Pelvis { get; set; } // Ginecoide, Estrecha, Límite
    public string? VariedadPresentacion { get; set; }
}