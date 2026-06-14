using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendProductEmailResponse
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("message_id")]
    public string? MessageId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}
