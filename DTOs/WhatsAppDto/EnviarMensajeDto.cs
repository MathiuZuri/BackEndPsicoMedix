using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.DTOs.WhatsAppDto;

public class EnviarMensajeDto
{
    [Required]
    public Guid ChatId { get; set; }

    [Required]
    [StringLength(4000, ErrorMessage = "El mensaje es demasiado largo.")]
    public string Texto { get; set; } = string.Empty;
}