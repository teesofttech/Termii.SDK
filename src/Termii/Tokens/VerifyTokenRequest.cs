using System.Text.Json.Serialization;

namespace Termii;

public sealed class VerifyTokenRequest
{
    [JsonPropertyName("pin_id")]
    public string PinId { get; set; } = string.Empty;

    [JsonPropertyName("pin")]
    public string Pin { get; set; } = string.Empty;

    internal void Validate()
    {
        TermiiRequestValidation.Required(PinId, nameof(PinId));
        TermiiRequestValidation.Required(Pin, nameof(Pin));
    }
}
