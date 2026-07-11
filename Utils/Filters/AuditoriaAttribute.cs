using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Utils.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuditoriaAttribute : Attribute
{
    public AuditoriaAttribute(
        string modulo,
        string entidad,
        TipoAccionAuditoria tipoAccion,
        NivelAuditoria nivel = NivelAuditoria.Normal)
    {
        Modulo = modulo;
        Entidad = entidad;
        TipoAccion = tipoAccion;
        Nivel = nivel;
    }

    public string Modulo { get; }
    public string Entidad { get; }
    public TipoAccionAuditoria TipoAccion { get; }
    public NivelAuditoria Nivel { get; }
}