using System.Text.Json.Serialization;

namespace Termii;

public sealed class TermiiMessageMedia
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("caption")]
    public string? Caption { get; set; }
}
