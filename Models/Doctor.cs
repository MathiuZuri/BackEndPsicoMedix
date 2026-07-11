using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class Doctor
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string CodigoDoctor { get; set; } = string.Empty;
    public string CMP { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;

    public string Especialidad { get; set; } = string.Empty;
    public string? Celular { get; set; }
    public string? Correo { get; set; }

    public DateTime FechaInicioContrato { get; set; }
    public DateTime? FechaFinContrato { get; set; }

    public EstadoDoctor Estado { get; set; } = EstadoDoctor.Activo;

    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public ICollection<HorarioDoctor> Horarios { get; set; } = new List<HorarioDoctor>();
    public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    public ICollection<Atencion> Atenciones { get; set; } = new List<Atencion>();
}