using System.Text.Json.Serialization;

namespace Termii;

public sealed class CreatePhonebookRequest
{
    [JsonPropertyName("phonebook_name")]
    public string PhonebookName { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    internal void Validate()
    {
        TermiiRequestValidation.Required(PhonebookName, nameof(PhonebookName));
    }
}
