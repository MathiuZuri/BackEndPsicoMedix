using psicomedixMonolito.DTOs.Atenciones;

namespace psicomedixMonolito.Services.ATENCIONES;

public interface IPsicoFormularioService
{
    /// <summary>
    /// Procesa de manera unificada el guardado o actualización parcial/total de las 6 secciones psicológicas.
    /// </summary>
    Task ActualizarFormularioPsicologicoAsync(Guid atencionId, RegistrarAtencionDto dto);
}