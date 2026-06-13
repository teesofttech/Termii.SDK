using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class CheckDndResponse
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("network")]
    public string? Network { get; set; }

    [JsonPropertyName("dnd_active")]
    public bool? DndActive { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}
