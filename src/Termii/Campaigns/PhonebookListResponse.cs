using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class PhonebookListResponse
{
    [JsonPropertyName("content")]
    public IReadOnlyList<PhonebookRecord> Content { get; set; } = Array.Empty<PhonebookRecord>();

    [JsonPropertyName("data")]
    public IReadOnlyList<PhonebookRecord>? Data { get; set; }

    [JsonPropertyName("totalElements")]
    public int? TotalElements { get; set; }

    [JsonPropertyName("totalPages")]
    public int? TotalPages { get; set; }

    [JsonPropertyName("size")]
    public int? Size { get; set; }

    [JsonPropertyName("number")]
    public int? Number { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}
