using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendBulkMessageRequest
{
    [JsonPropertyName("to")]
    public IReadOnlyCollection<string> To { get; set; } = Array.Empty<string>();

    [JsonPropertyName("from")]
    public string From { get; set; } = string.Empty;

    [JsonPropertyName("sms")]
    public string Sms { get; set; } = string.Empty;

    [JsonIgnore]
    public TermiiMessageType Type { get; set; } = TermiiMessageType.Plain;

    [JsonIgnore]
    public TermiiMessageChannel Channel { get; set; } = TermiiMessageChannel.Generic;

    internal void Validate()
    {
        if (To is null || To.Count == 0)
        {
            throw new ArgumentException("At least one recipient phone number is required.", nameof(To));
        }

        foreach (var recipient in To)
        {
            TermiiRequestValidation.Required(recipient, nameof(To));
        }

        TermiiRequestValidation.Required(From, nameof(From));
        TermiiRequestValidation.Required(Sms, nameof(Sms));
    }
}
