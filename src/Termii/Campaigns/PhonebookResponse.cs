using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class PhonebookResponse
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("data")]
    public PhonebookRecord? Data { get; set; }

    [JsonPropertyName("phonebook")]
    public PhonebookRecord? Phonebook { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}
