using psicomedixMonolito.DTOs.Comprobantes;

namespace psicomedixMonolito.Services.Interfacespdf;

// Interfaz del servicio en la raíz de Services para mantener la estructura limpia
public interface IComprobantePdfService
{
    byte[] GenerarBoletaPagoPdf(ComprobantePagoPreviewDto dto);
    byte[] GenerarConstanciaCitaPdf(ComprobanteCitaPreviewDto dto);
    byte[] GenerarResumenAtencionPdf(ComprobanteAtencionPreviewDto dto);
    byte[] GenerarEstadoCuentaPacientePdf(ComprobanteEstadoCuentaPreviewDto dto);
}