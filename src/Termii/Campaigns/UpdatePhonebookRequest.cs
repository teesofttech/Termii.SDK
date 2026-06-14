using System.Text.Json.Serialization;

namespace Termii;

public sealed class UpdatePhonebookRequest
{
    [JsonPropertyName("phonebook_name")]
    public string? PhonebookName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
