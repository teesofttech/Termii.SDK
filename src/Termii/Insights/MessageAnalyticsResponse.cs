using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class MessageAnalyticsResponse
{
    [JsonPropertyName("sent")]
    public int? Sent { get; set; }

    [JsonPropertyName("delivered")]
    public int? Delivered { get; set; }

    [JsonPropertyName("failed")]
    public int? Failed { get; set; }

    [JsonPropertyName("pending")]
    public int? Pending { get; set; }

    [JsonPropertyName("rejected")]
    public int? Rejected { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}
