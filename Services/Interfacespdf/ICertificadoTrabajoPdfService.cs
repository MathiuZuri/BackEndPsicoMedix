using psicomedixMonolito.DTOs.PDFsDto;

namespace psicomedixMonolito.Services.Interfacespdf;

public interface ICertificadoTrabajoPdfService
{
    byte[] GeneratePdf(CertificadoTrabajoDto dto);
}