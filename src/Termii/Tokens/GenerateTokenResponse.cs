using System.Text.Json.Serialization;

namespace Termii;

public sealed class GenerateTokenResponse
{
    [JsonPropertyName("phone_number_other")]
    public string? PhoneNumberOther { get; set; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("otp")]
    public string? Otp { get; set; }

    [JsonPropertyName("pin_id")]
    public string? PinId { get; set; }
}
