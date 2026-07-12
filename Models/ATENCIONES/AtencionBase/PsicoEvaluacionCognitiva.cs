namespace psicomedixMonolito.Models.ATENCIONES.AtencionBase;

public class PsicoEvaluacionCognitiva
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AtencionId { get; set; }
    public Atencion Atencion { get; set; } = null!;

    // Triada de Beck
    public string? BeckPersonal { get; set; }
    public string? BeckMundoExterior { get; set; }
    public string? BeckFuturo { get; set; }
    public string? BeckAutolesiones { get; set; }
    public string? BeckAutolisis { get; set; }
    public string? BeckOtros { get; set; }

    // Funciones Cognitivas Superiores
    public string? FcPensamiento { get; set; }
    public string? FcAtencion { get; set; }
    public string? FcConcentracion { get; set; } // Ajustado sin tilde para evitar problemas de compilación
    public string? FcLenguaje { get; set; }
    public string? FcPercepcion { get; set; }
    public string? FcJuicio { get; set; }
    public string? FcAbstraccion { get; set; }
    public string? FcAprendizaje { get; set; }
    public string? FcMemoria { get; set; }
    public string? FcMotivacion { get; set; }
    public string? FcEmocion { get; set; }
    public string? FcCalculo { get; set; }
    public string? FcCoordinacionMotoraFina { get; set; }
    public string? FcCoordinacionMotoraGruesa { get; set; }
}