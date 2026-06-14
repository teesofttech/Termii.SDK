using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class PhonebookRecord
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("phonebook_id")]
    public string? PhonebookId { get; set; }

    [JsonPropertyName("phonebook_name")]
    public string? PhonebookName { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("total_contacts")]
    public int? TotalContacts { get; set; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}
