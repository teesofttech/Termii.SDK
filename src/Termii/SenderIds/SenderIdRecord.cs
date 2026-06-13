using System.Text.Json.Serialization;

namespace Termii;

public sealed class SenderIdRecord
{
    [JsonPropertyName("sender_id")]
    public string? SenderId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("company")]
    public string? Company { get; set; }

    [JsonPropertyName("usecase")]
    public string? UseCase { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }
}
