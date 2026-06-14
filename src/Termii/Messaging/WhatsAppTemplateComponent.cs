using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class WhatsAppTemplateComponent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("parameters")]
    public IReadOnlyCollection<Dictionary<string, JsonElement>>? Parameters { get; set; }
}
