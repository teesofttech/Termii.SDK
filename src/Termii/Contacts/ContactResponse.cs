using System.Text.Json.Serialization;

namespace Termii;

public sealed class ContactResponse
{
    [JsonPropertyName("Contact added successfully")]
    public ContactRecord? ContactAddedSuccessfully { get; set; }

    public string? Message { get; set; }

    public ContactRecord? Data { get; set; }
}
