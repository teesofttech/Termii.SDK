using System.Text.Json.Serialization;

namespace Termii;

public sealed class GenerateTokenRequest
{
    [JsonIgnore]
    public TermiiTokenPinType PinType { get; set; } = TermiiTokenPinType.Numeric;

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("pin_attempts")]
    public int PinAttempts { get; set; }

    [JsonPropertyName("pin_time_to_live")]
    public int PinTimeToLive { get; set; }

    [JsonPropertyName("pin_length")]
    public int PinLength { get; set; }

    internal void Validate()
    {
        TermiiRequestValidation.Required(PhoneNumber, nameof(PhoneNumber));
        TermiiRequestValidation.Positive(PinAttempts, nameof(PinAttempts));
        TermiiRequestValidation.Range(PinTimeToLive, 0, 60, nameof(PinTimeToLive));
        TermiiRequestValidation.Range(PinLength, 4, 8, nameof(PinLength));
    }
}
