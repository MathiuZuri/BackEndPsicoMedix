using psicomedixMonolito.DTOs.Comprobantes;

namespace psicomedixMonolito.Services;

public interface IComprobanteService
{
    Task<ComprobantePagoPreviewDto> PreviewBoletaPagoAsync(Guid pagoId, decimal? tasaImpuesto = null);
    Task<Guid> EmitirBoletaPagoAsync(EmitirComprobantePagoDto dto, Guid usuarioId);

    Task<ComprobanteCitaPreviewDto> PreviewConstanciaCitaAsync(Guid citaId);
    Task<Guid> EmitirConstanciaCitaAsync(EmitirComprobanteCitaDto dto, Guid usuarioId);

    Task<ComprobanteAtencionPreviewDto> PreviewResumenAtencionAsync(Guid atencionId);
    Task<Guid> EmitirResumenAtencionAsync(EmitirComprobanteAtencionDto dto, Guid usuarioId);

    Task<ComprobanteEstadoCuentaPreviewDto> PreviewEstadoCuentaPacienteAsync(Guid pacienteId);
    Task<Guid> EmitirEstadoCuentaPacienteAsync(EmitirComprobanteEstadoCuentaDto dto, Guid usuarioId);

    Task<ComprobanteDto> ObtenerPorIdAsync(Guid id);
    Task<IEnumerable<ComprobanteDto>> ObtenerPorPacienteAsync(Guid pacienteId);
    Task<IEnumerable<ComprobanteDto>> ObtenerPorPagoAsync(Guid pagoId);
    Task<IEnumerable<ComprobanteDto>> ObtenerPorAtencionAsync(Guid atencionId);
    Task<IEnumerable<ComprobanteDto>> ObtenerTodosAsync();
    Task AnularComprobanteAsync(Guid comprobanteId, string motivo, Guid usuarioId);
    
    Task<ArchivoDescargaDto> GenerarPdfBoletaPagoAsync(Guid comprobanteId);
    Task<ArchivoDescargaDto> GenerarPdfConstanciaCitaAsync(Guid comprobanteId);
    Task<ArchivoDescargaDto> GenerarPdfResumenAtencionAsync(Guid comprobanteId);
    Task<ArchivoDescargaDto> GenerarPdfEstadoCuentaPacienteAsync(Guid comprobanteId);
}