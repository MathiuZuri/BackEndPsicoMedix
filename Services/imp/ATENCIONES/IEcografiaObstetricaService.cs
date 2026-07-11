using psicomedixMonolito.DTOs.Atenciones.Modulos;

namespace psicomedixMonolito.Services.imp.ATENCIONES;

public interface IEcografiaObstetricaService
{
    Task<IEnumerable<EcografiaObstetricaDto>> ObtenerPorAtencionAsync(Guid atencionId);
    Task<Guid> RegistrarAsync(Guid atencionId, EcografiaObstetricaDto dto);
}