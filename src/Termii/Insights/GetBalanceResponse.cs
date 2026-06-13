using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class GetBalanceResponse
{
    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("balance")]
    public decimal? Balance { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}
