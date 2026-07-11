using psicomedixMonolito.DTOs.PDFsDto;

namespace psicomedixMonolito.Services.Interfacespdf;

public interface IResumenPartoPdfService
{
    byte[] GeneratePdf(ResumenPartoPdfDto dto);
}