using psicomedixMonolito.DTOs.Atenciones.Modulos;

namespace psicomedixMonolito.Services.imp.ATENCIONES;

public interface ITactoVaginalService
{
    Task<IEnumerable<TactoVaginalDto>> ObtenerPorAtencionAsync(Guid atencionId);
    Task<Guid> RegistrarAsync(Guid atencionId, TactoVaginalDto dto);
}