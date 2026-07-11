namespace psicomedixMonolito.DTOs.Comprobantes;

public class DocumentoGeneradoDto
{
    public string NombreArchivo { get; set; } = string.Empty;

    public string ContentType { get; set; } = "application/pdf";

    public byte[] Archivo { get; set; } = Array.Empty<byte>();
}