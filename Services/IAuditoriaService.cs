using psicomedixMonolito.DTOs.Auditoria;
using psicomedixMonolito.DTOs.Comunes;
using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Services;

public interface IAuditoriaService
{
    Task<IEnumerable<AuditoriaResponseDto>> ObtenerTodosAsync();
    Task<IEnumerable<AuditoriaResponseDto>> ObtenerPorUsuarioAsync(Guid usuarioId);

    Task<PaginacionResponseDto<AuditoriaResponseDto>> ObtenerTodosPaginadosAsync(
        PaginacionRequestDto request,
        TipoAccionAuditoria? tipoAccion = null,
        bool? soloConsultas = null
    );

    Task<PaginacionResponseDto<AuditoriaResponseDto>> ObtenerPorUsuarioPaginadosAsync(
        Guid usuarioId,
        PaginacionRequestDto request,
        TipoAccionAuditoria? tipoAccion = null,
        bool? soloConsultas = null
    );
}