namespace psicomedixMonolito.Utils.Configurations;

// esto es exclusivo de evolution api, no incluir al sistema
public class WhatsAppOptions
{
    public string Provider { get; set; } = "EvolutionApi";

    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;

    public string InstanceName { get; set; } = string.Empty;

    public bool Enabled { get; set; }

    public int ReminderHoursBefore { get; set; } = 24;

    public int CheckIntervalMinutes { get; set; } = 5;

    public int MaxIntentos { get; set; } = 3;
    
    public string WebhookSecretToken { get; set; } = string.Empty;
}