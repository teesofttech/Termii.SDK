using System.Text.Json.Serialization;

namespace Termii;

public sealed class VerifyTokenResponse
{
    [JsonPropertyName("pinId")]
    public string? PinId { get; set; }

    [JsonPropertyName("verified")]
    public string? Verified { get; set; }

    [JsonPropertyName("msisdn")]
    public string? Msisdn { get; set; }
}
