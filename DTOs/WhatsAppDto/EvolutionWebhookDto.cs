namespace psicomedixMonolito.DTOs.WhatsAppDto;

public class EvolutionWebhookDto
{
    public string Event { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public WebhookData Data { get; set; } = null!;
}

public class WebhookData
{
    public WebhookKey Key { get; set; } = null!;
    public WebhookMessage? Message { get; set; }
    public string? PushName { get; set; }
    public long MessageTimestamp { get; set; }
}

public class WebhookKey
{
    public string Id { get; set; } = string.Empty;
    public string RemoteJid { get; set; } = string.Empty;
    public string? RemoteJidAlt { get; set; } // ◄── ¡AGREGADO PARA EVITAR LA COLISIÓN!
    public bool FromMe { get; set; }
}

public class WebhookMessage
{
    // Texto plano de conversaciones normales
    public string? Conversation { get; set; }
    
    // Texto de mensajes que vienen con imágenes/videos/archivos
    public ExtendedTextMessage? ExtendedTextMessage { get; set; }
}

public class ExtendedTextMessage
{
    public string? Text { get; set; }
}