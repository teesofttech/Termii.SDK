using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public class SendWhatsAppTemplateRequest
{
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("device_id")]
    public string DeviceId { get; set; } = string.Empty;

    [JsonPropertyName("template_id")]
    public string TemplateId { get; set; } = string.Empty;

    [JsonPropertyName("template_name")]
    public string? TemplateName { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("variables")]
    public Dictionary<string, JsonElement>? Variables { get; set; }

    [JsonPropertyName("components")]
    public IReadOnlyCollection<WhatsAppTemplateComponent>? Components { get; set; }

    internal virtual void Validate()
    {
        TermiiRequestValidation.Required(PhoneNumber, nameof(PhoneNumber));
        TermiiRequestValidation.Required(DeviceId, nameof(DeviceId));
        TermiiRequestValidation.Required(TemplateId, nameof(TemplateId));
    }
}
