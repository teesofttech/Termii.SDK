using System.Text.Json.Serialization;

namespace Termii;

public sealed class VoiceCallTokenRequest
{
    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    internal void Validate()
    {
        TermiiRequestValidation.Required(PhoneNumber, nameof(PhoneNumber));
        TermiiRequestValidation.Required(Code, nameof(Code));
    }
}
