using System.Text.Json.Serialization;

namespace Termii;

public sealed class WhatsAppTemplateMedia
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("caption")]
    public string? Caption { get; set; }

    [JsonPropertyName("filename")]
    public string? FileName { get; set; }

    internal void Validate()
    {
        TermiiRequestValidation.Required(Type, nameof(Type));
        TermiiRequestValidation.Required(Url, nameof(Url));
    }
}
