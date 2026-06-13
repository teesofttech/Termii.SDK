using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendTokenRequest
{
    [JsonIgnore]
    public TermiiTokenPinType PinType { get; set; } = TermiiTokenPinType.Numeric;

    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;

    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;

    [JsonIgnore]
    public TermiiMessageChannel Channel { get; set; } = TermiiMessageChannel.Generic;

    [JsonPropertyName("pin_attempts")]
    public int PinAttempts { get; set; }

    [JsonPropertyName("pin_time_to_live")]
    public int PinTimeToLive { get; set; }

    [JsonPropertyName("pin_length")]
    public int PinLength { get; set; }

    [JsonPropertyName("pin_placeholder")]
    public string PinPlaceholder { get; set; } = string.Empty;

    [JsonPropertyName("message_text")]
    public string MessageText { get; set; } = string.Empty;

    internal void Validate()
    {
        TermiiRequestValidation.Required(To, nameof(To));
        TermiiRequestValidation.Required(From, nameof(From));
        TermiiRequestValidation.Required(PinPlaceholder, nameof(PinPlaceholder));
        TermiiRequestValidation.Required(MessageText, nameof(MessageText));
        TermiiRequestValidation.Positive(PinAttempts, nameof(PinAttempts));
        TermiiRequestValidation.Range(PinTimeToLive, 0, 60, nameof(PinTimeToLive));
        TermiiRequestValidation.Range(PinLength, 4, 8, nameof(PinLength));
    }
}
