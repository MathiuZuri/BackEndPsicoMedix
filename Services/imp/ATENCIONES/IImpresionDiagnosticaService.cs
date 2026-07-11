using psicomedixMonolito.DTOs.Atenciones.Modulos;

namespace psicomedixMonolito.Services.imp.ATENCIONES;

public interface IImpresionDiagnosticaService
{
    Task<ImpresionDiagnosticaDto?> ObtenerPorAtencionAsync(Guid atencionId);
    Task<Guid> RegistrarAsync(Guid atencionId, ImpresionDiagnosticaDto dto);
}