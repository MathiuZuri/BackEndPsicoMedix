using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Models;

public class Auditoria
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public TipoAccionAuditoria TipoAccion { get; set; }

    public string Modulo { get; set; } = string.Empty;
    public string EntidadAfectada { get; set; } = string.Empty;
    public Guid? EntidadId { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    public bool FueExitoso { get; set; } = true;
    public string? DetalleError { get; set; }
    
    public NivelAuditoria Nivel { get; set; } = NivelAuditoria.Normal;

    public DateTime FechaHora { get; set; } = DateTime.UtcNow;
    public bool EsConsulta { get; set; }
}