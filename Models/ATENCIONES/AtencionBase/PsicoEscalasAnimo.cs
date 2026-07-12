namespace psicomedixMonolito.Models.ATENCIONES.AtencionBase;

public class PsicoEscalasAnimo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    public int? EscalaIrritabilidad { get; set; }
    public int? EscalaTristeza { get; set; }
    public int? EscalaAnsiedad { get; set; }
    public int? EscalaPreocupacion { get; set; }
    public int? EscalaImpulsividad { get; set; }
    public int? EscalaEstres { get; set; }
    public int? EscalaFatiga { get; set; }
    public int? EscalaDesmotivacion { get; set; }
}