using psicomedixMonolito.Enums;

namespace psicomedixMonolito.Models;

public class PacienteFamiliar
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public ParentescoFamiliar? Parentesco { get; set; }
    public string? Nombres { get; set; }
    public int? Edad { get; set; }
    public string? Ocupacion { get; set; }
    public bool? VivenJuntos { get; set; }
}