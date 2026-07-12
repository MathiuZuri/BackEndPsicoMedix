namespace psicomedixMonolito.Models.ATENCIONES.AtencionBase;

public class PsicoDesarrolloPsicosocial
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    public string? AutoestimaAutocuidado { get; set; }
    public string? AcademicoLaboral { get; set; }
    public string? SocializacionFamilia { get; set; }
    public string? PersonalidadAutoexpresion { get; set; }
}