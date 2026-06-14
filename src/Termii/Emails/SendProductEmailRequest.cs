using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendProductEmailRequest
{
    [JsonPropertyName("email_address")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("template_id")]
    public string TemplateId { get; set; } = string.Empty;

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("from")]
    public string? From { get; set; }

    [JsonPropertyName("sender_name")]
    public string? SenderName { get; set; }

    [JsonPropertyName("data")]
    public Dictionary<string, JsonElement>? Data { get; set; }

    internal void Validate()
    {
        TermiiRequestValidation.Required(EmailAddress, nameof(EmailAddress));
        TermiiRequestValidation.Required(TemplateId, nameof(TemplateId));
    }
}
