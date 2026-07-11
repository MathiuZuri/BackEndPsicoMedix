namespace psicomedixMonolito.DTOs.Comprobantes;

public class ArchivoDescargaDto
{
    public byte[] Archivo { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = "application/pdf";
    public string NombreArchivo { get; set; } = string.Empty;
}