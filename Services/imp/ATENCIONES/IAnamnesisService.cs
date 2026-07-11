using psicomedixMonolito.DTOs.Atenciones.Modulos;

namespace psicomedixMonolito.Services.imp.ATENCIONES;

public interface IAnamnesisService
{
    Task<AnamnesisDto?> ObtenerPorAtencionAsync(Guid atencionId);
    Task<Guid> RegistrarAsync(Guid atencionId, AnamnesisDto dto);
}