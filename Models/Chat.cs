using System.ComponentModel.DataAnnotations;

namespace psicomedixMonolito.Models;

public class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string TelefonoWhatsApp { get; set; } = string.Empty;

    public string NombreContacto { get; set; } = string.Empty;

    public string UltimoMensaje { get; set; } = string.Empty;

    public DateTime FechaUltimaInteraccion { get; set; } = DateTime.UtcNow;

    public int MensajesNoLeidos { get; set; }

    // Relación opcional: El chat puede pertenecer a un paciente registrado
    public Guid? PacienteId { get; set; }
    public Paciente? Paciente { get; set; }

    // Propiedad de navegación para el historial de burbujas
    public ICollection<MensajeChat> Mensajes { get; set; } = new List<MensajeChat>();
}