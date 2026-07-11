using psicomedixMonolito.Enums;
using psicomedixMonolito.Models.ATENCIONES;

namespace psicomedixMonolito.Models;

public class AjusteFinanciero
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PagoId { get; set; }
    public Pago Pago { get; set; } = null!;

    public Guid? AtencionId { get; set; }
    public Atencion? Atencion { get; set; }

    public Guid PacienteId { get; set; }
    public Paciente Paciente { get; set; } = null!;

    public TipoAjusteFinanciero TipoAjuste { get; set; }

    public decimal MontoAjuste { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public string? Observacion { get; set; }

    public Guid? UsuarioRegistroId { get; set; }
    public Usuario? UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}