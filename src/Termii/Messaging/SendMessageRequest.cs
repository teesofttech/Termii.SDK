using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendMessageRequest
{
    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;

    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;

    [JsonPropertyName("sms")]
    public string Sms { get; set; } = string.Empty;

    [JsonIgnore]
    public TermiiMessageType Type { get; set; } = TermiiMessageType.Plain;

    [JsonIgnore]
    public TermiiMessageChannel Channel { get; set; } = TermiiMessageChannel.Generic;

    [JsonPropertyName("media")]
    public TermiiMessageMedia? Media { get; set; }

    internal void Validate()
    {
        TermiiRequestValidation.Required(To, nameof(To));
        TermiiRequestValidation.Required(From, nameof(From));
        TermiiRequestValidation.Required(Sms, nameof(Sms));
    }
}
