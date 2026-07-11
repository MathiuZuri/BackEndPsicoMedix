using psicomedixMonolito.Enums;

namespace psicomedixMonolito.DTOs.Auditoria;

public class AuditoriaResponseDto
{
    public Guid Id { get; set; }

    public Guid? UsuarioId { get; set; }
    public string? UsuarioNombre { get; set; }

    public TipoAccionAuditoria TipoAccion { get; set; }

    public string Modulo { get; set; } = string.Empty;
    public string EntidadAfectada { get; set; } = string.Empty;
    public Guid? EntidadId { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    public bool FueExitoso { get; set; }
    public string? DetalleError { get; set; }
    
    public NivelAuditoria Nivel { get; set; }

    public DateTime FechaHora { get; set; }
    
    public bool EsConsulta { get; set; } // ✅ nuevo campo
}