using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendEmailTokenRequest
{
    [JsonPropertyName("email_address")]
    public string EmailAddress { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("email_configuration_id")]
    public string EmailConfigurationId { get; set; } = string.Empty;

    internal void Validate()
    {
        TermiiRequestValidation.Required(EmailAddress, nameof(EmailAddress));
        TermiiRequestValidation.Required(Code, nameof(Code));
        TermiiRequestValidation.Required(EmailConfigurationId, nameof(EmailConfigurationId));
    }
}
