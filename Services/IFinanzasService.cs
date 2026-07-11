using psicomedixMonolito.DTOs.Comunes;
using psicomedixMonolito.DTOs.Finanzas;

namespace psicomedixMonolito.Services;

public interface IFinanzasService
{
    Task<ResumenDiarioFinanzasDto> ObtenerResumenDiarioAsync(DateOnly fecha);
    Task<ResumenMensualFinanzasDto> ObtenerResumenMensualAsync(int anio, int mes);
    Task<ResumenAnualFinanzasDto> ObtenerResumenAnualAsync(int anio);

    Task<IEnumerable<PagoFinanzasDto>> ObtenerPagosPendientesAsync();
    Task<IEnumerable<PagoFinanzasDto>> ObtenerPagosPagadosAsync();
    Task<IEnumerable<PagoFinanzasDto>> ObtenerPagosParcialesAsync();

    Task<PagoFinanzasDto?> ObtenerPagoPorCodigoAsync(string codigoPago);
    Task<EstadoCuentaPacienteDto> ObtenerEstadoCuentaPacienteAsync(Guid pacienteId);

    Task<IEnumerable<EstadoPagoAtencionDto>> ObtenerDeudasRealesAsync();
    Task<IEnumerable<EstadoPagoAtencionDto>> ObtenerDeudasRealesPacienteAsync(Guid pacienteId);
    Task<EstadoPagoAtencionDto> ObtenerEstadoPagoAtencionAsync(Guid atencionId);
    
    Task<IEnumerable<PagoFinanzasDto>> ObtenerLibroDiarioAsync(DateOnly fecha);
    Task<ResumenFinancieroMensualCompletoDto> ObtenerResumenFinancieroMensualCompletoAsync(int anio, int mes);
    
    Task<Guid> RegistrarAjusteFinancieroAsync(RegistrarAjusteFinancieroDto dto, Guid usuarioId);

    Task<IEnumerable<AjusteFinancieroDto>> ObtenerAjustesFinancierosAsync();
    Task<IEnumerable<AjusteFinancieroDto>> ObtenerAjustesPorAtencionAsync(Guid atencionId);
    Task<IEnumerable<AjusteFinancieroDto>> ObtenerAjustesPorPagoAsync(Guid pagoId);
    
    Task<PaginacionResponseDto<PagoFinanzasDto>> ObtenerPagosPendientesPaginadosAsync(PaginacionRequestDto request);
    Task<EstadoPagoAtencionDto> ObtenerEstadoPagoAtencionDetalladoAsync(Guid atencionId);
}