using psicomedixMonolito.DTOs.PDFsDto;

namespace psicomedixMonolito.Services.Interfacespdf;

public interface IHistoriaClinicaPdfService
{
    byte[] GeneratePdf(HistoriaClinicaPdfDto dto);
}