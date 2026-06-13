using System.Text.Json.Serialization;

namespace Termii;

public sealed class SenderIdListResponse
{
    [JsonPropertyName("content")]
    public IReadOnlyCollection<SenderIdRecord> Content { get; set; } = Array.Empty<SenderIdRecord>();

    [JsonPropertyName("totalElements")]
    public int? TotalElements { get; set; }

    [JsonPropertyName("totalPages")]
    public int? TotalPages { get; set; }

    [JsonPropertyName("size")]
    public int? Size { get; set; }

    [JsonPropertyName("number")]
    public int? Number { get; set; }

    [JsonPropertyName("first")]
    public bool? First { get; set; }

    [JsonPropertyName("last")]
    public bool? Last { get; set; }

    [JsonPropertyName("empty")]
    public bool? Empty { get; set; }
}
