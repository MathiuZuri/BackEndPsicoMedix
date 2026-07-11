using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using psicomedixMonolito.Services.imp.WhastAppImp;
using psicomedixMonolito.Utils.Configurations;

namespace psicomedixMonolito.Services.imp.WhatsApp;
// esto es exclusivo de evolution api, no incluir al sistema
public class EvolutionWhatsAppService : INotificacionWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly WhatsAppOptions _options;
    private readonly ILogger<EvolutionWhatsAppService> _logger;

    public EvolutionWhatsAppService(
        HttpClient httpClient,
        IOptions<WhatsAppOptions> options,
        ILogger<EvolutionWhatsAppService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task EnviarMensajeAsync(string telefonoDestino, string mensaje, CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled) return;

        // Ejecuta la normalización y la validación de seguridad de C#
        var telefonoNormalizado = NormalizarTelefonoPeru(telefonoDestino);

        var payload = new { number = telefonoNormalizado, text = mensaje };
        var json = JsonSerializer.Serialize(payload);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"/message/sendText/{_options.InstanceName}");
        request.Headers.Add("apikey", _options.ApiKey);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Error enviando WhatsApp. StatusCode: {StatusCode}. Body: {Body}", response.StatusCode, responseBody);
            throw new InvalidOperationException($"Error enviando mensaje por WhatsApp: {response.StatusCode}");
        }
    }

    private string NormalizarTelefonoPeru(string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono)) return string.Empty;

        // 1. Si es un JID de privacidad (@lid), pasa directo de forma nativa e inmediata
        if (telefono.Contains("@lid"))
        {
            return telefono; 
        }

        // 2. Si es un JID tradicional, le quitamos el dominio para validar los dígitos puros
        if (telefono.Contains("@s.whatsapp.net"))
        {
            telefono = telefono.Split('@')[0];
        }

        var limpio = new string(telefono.Where(char.IsDigit).ToArray());

        // 3.  ESCUDO DE SEGURIDAD EN C#: Validar formato de celular peruano válido
        // Debe tener 9 dígitos y empezar con 9, o tener 11 dígitos y empezar con 519
        bool esCelularValido = (limpio.Length == 9 && limpio.StartsWith("9")) || 
                               (limpio.Length == 11 && limpio.StartsWith("519"));

        if (!esCelularValido)
        {
            _logger.LogError("[Escudo] Envío bloqueado en C# para proteger el chip. Formato inválido o teléfono fijo detectado: {Telefono}", telefono);
            throw new InvalidOperationException("El número de teléfono no tiene un formato válido de celular de WhatsApp (Perú).");
        }

        // 4. Agregar código de país si tiene 9 dígitos
        if (limpio.Length == 9)
            return $"51{limpio}";

        return limpio;
    }
    
    public async Task<string?> ObtenerQrInstanciaAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("apikey", _options.ApiKey);

            var url = $"/instance/connect/{_options.InstanceName}";
            
            var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error al solicitar QR a Evolution. Status: {Status}", response.StatusCode);
                return null;
            }

            var jsonText = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(jsonText);
            var root = doc.RootElement;

            // ====================================================================
            // 1. COMPROBACIÓN DE ESTADO MULTI-VERSION (Soporta "connected" y "open")
            // ====================================================================
            string? valorEstado = null;

            // Buscar en la raíz del JSON
            if (root.TryGetProperty("state", out var stateDirecto)) 
                valorEstado = stateDirecto.GetString();
            else if (root.TryGetProperty("status", out var statusDirecto)) 
                valorEstado = statusDirecto.GetString();
            
            // Buscar anidado dentro del objeto "instance"
            else if (root.TryGetProperty("instance", out var instObj) && instObj.ValueKind == JsonValueKind.Object)
            {
                if (instObj.TryGetProperty("state", out var stateAnidado)) 
                    valorEstado = stateAnidado.GetString();
                else if (instObj.TryGetProperty("status", out var statusAnidado)) 
                    valorEstado = statusAnidado.GetString();
            }

            // Si Evolution responde que está "open" o "connected", el canal está listo para usar
            if (string.Equals(valorEstado, "open", StringComparison.OrdinalIgnoreCase) || 
                string.Equals(valorEstado, "connected", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("[whatsapp] Conexión validada con éxito. Estado actual de la instancia: {Estado}", valorEstado);
                return "CONNECTED";
            }

            // ====================================================================
            // 2. EXTRACCIÓN DEL BASE64 DEL CÓDIGO QR (Si no está conectado)
            // ====================================================================
            if (root.TryGetProperty("base64", out var base64Raiz))
            {
                return base64Raiz.GetString();
            }

            if (root.TryGetProperty("qrcode", out var qrProp) && qrProp.ValueKind == JsonValueKind.Object && qrProp.TryGetProperty("base64", out var base64Qr))
            {
                return base64Qr.GetString();
            }

            if (root.TryGetProperty("instance", out var instanceProp) && instanceProp.ValueKind == JsonValueKind.Object)
            {
                if (instanceProp.TryGetProperty("qrcode", out var instQr) && instQr.ValueKind == JsonValueKind.Object && instQr.TryGetProperty("base64", out var base64InstQr))
                {
                    return base64InstQr.GetString();
                }
                if (instanceProp.TryGetProperty("base64", out var base64InstDirecto))
                {
                    return base64InstDirecto.GetString();
                }
            }

            _logger.LogWarning("[whatsapp] Respuesta de Evolution recibida, pero estructura desconocida. JSON: {Json}", jsonText);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción al intentar recuperar el QR de WhatsApp desde el microservicio.");
            return null;
        }
    }
    
    // Consulta a la red de WhatsApp el perfil real detrás del LID
    public async Task<string?> BuscarContactoEnAgendaAsync(string jidLid, CancellationToken cancellationToken = default)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("apikey", _options.ApiKey);

            var url = $"/contact/find/{_options.InstanceName}?number={Uri.EscapeDataString(jidLid)}";
    
            var response = await _httpClient.GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode) return null;

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            string? jidReal = null;
            if (root.TryGetProperty("id", out var idProp)) jidReal = idProp.GetString();
            else if (root.TryGetProperty("jid", out var jidPropAlt)) jidReal = jidPropAlt.GetString();

            if (!string.IsNullOrEmpty(jidReal) && jidReal.Contains("@s.whatsapp.net"))
            {
                _logger.LogInformation("[whatsapp Agenda] ¡Éxito! El LID {Lid} corresponde al número real: {JidReal}", jidLid, jidReal);
                return jidReal;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar el LID {Lid} en la agenda de Evolution", jidLid);
            return null;
        }
    }
}