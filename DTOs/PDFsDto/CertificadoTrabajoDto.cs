// Domain/PDFsDto/CertificadoTrabajoDto.cs
namespace psicomedixMonolito.DTOs.PDFsDto;

public class CertificadoTrabajoDto
{
    public string NombreCompleto { get; set; } = "";
    public string Dni { get; set; } = "";
    public string CodigoUsuario { get; set; } = "";
    public string Correo { get; set; } = "";
    public List<string> Roles { get; set; } = new();
    public string Area { get; set; } = "";
    
    // ✅ NUEVO: Cargo específico (se deduce del rol o área)
    public string Cargo { get; set; } = "";
    
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string NombreDirector { get; set; } = "";
    public string CargoDirector { get; set; } = "";
    
    // ✅ NUEVO: Código único de validación
    public string CodigoCertificado { get; set; } = "";
    
    // ✅ NUEVO: Observaciones adicionales
    public string Observaciones { get; set; } = "";
}