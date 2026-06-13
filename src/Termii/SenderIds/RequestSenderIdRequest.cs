using System.Text.Json.Serialization;

namespace Termii;

public sealed class RequestSenderIdRequest
{
    [JsonPropertyName("sender_id")]
    public string SenderId { get; set; } = string.Empty;

    [JsonPropertyName("use_case")]
    public string UseCase { get; set; } = string.Empty;

    [JsonPropertyName("company")]
    public string Company { get; set; } = string.Empty;

    internal void Validate()
    {
        TermiiRequestValidation.Required(SenderId, nameof(SenderId));
        TermiiRequestValidation.Required(UseCase, nameof(UseCase));
        TermiiRequestValidation.Required(Company, nameof(Company));
    }
}
