using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.Models;

public class MensajeChat
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ChatId { get; set; }
    public Chat Chat { get; set; } = null!;

    [Required]
    public string Texto { get; set; } = string.Empty;

    // true: Enviado por la clínica | false: Enviado por el paciente
    public bool EsMio { get; set; }

    public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

    // ID único de WhatsApp para evitar duplicados por reintentos de red
    [Required]
    public string MessageIdWhatsApp { get; set; } = string.Empty;
}