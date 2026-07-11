using psicomedixMonolito.DTOs.Atenciones.Modulos;

namespace psicomedixMonolito.Services.imp.ATENCIONES;

public interface IExamenFisicoService
{
    Task<IEnumerable<ExamenFisicoDto>> ObtenerPorAtencionAsync(Guid atencionId);
    Task<Guid> RegistrarAsync(Guid atencionId, ExamenFisicoDto dto);
}