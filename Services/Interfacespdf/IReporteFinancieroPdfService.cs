using psicomedixMonolito.DTOs.PDFsDto;

namespace psicomedixMonolito.Services.Interfacespdf;

public interface IReporteFinancieroPdfService
{
    byte[] GeneratePdf(ReporteDiarioDto dto);
}