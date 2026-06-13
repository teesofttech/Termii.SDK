using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendNumberMessageRequest
{
    [JsonPropertyName("to")]
    public string To { get; set; } = string.Empty;

    [JsonPropertyName("sms")]
    public string Sms { get; set; } = string.Empty;

    internal void Validate()
    {
        TermiiRequestValidation.Required(To, nameof(To));
        TermiiRequestValidation.Required(Sms, nameof(Sms));
    }
}
