using System.Text.Json.Serialization;

namespace Termii;

public sealed class ContactListResponse
{
    public IReadOnlyList<string> Headers { get; set; } = Array.Empty<string>();

    public ContactPhonebookRecord? Phonebook { get; set; }

    public ContactListData? Data { get; set; }
}

public sealed class ContactListData
{
    public IReadOnlyList<ContactRecord> Content { get; set; } = Array.Empty<ContactRecord>();

    public int? TotalPages { get; set; }

    public int? TotalElements { get; set; }

    public int? Size { get; set; }

    public int? Number { get; set; }

    public bool? First { get; set; }

    public bool? Last { get; set; }

    public bool? Empty { get; set; }
}

public sealed class ContactPhonebookRecord
{
    public string? Id { get; set; }

    public string? ApplicationId { get; set; }

    public string? Description { get; set; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("phonebook_name")]
    public string? PhonebookName { get; set; }

    [JsonPropertyName("total_contact")]
    public int? TotalContact { get; set; }

    [JsonPropertyName("total_campaign")]
    public int? TotalCampaign { get; set; }
}

public sealed class ContactRecord
{
    public string? Id { get; set; }

    public string? Pid { get; set; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("email_address")]
    public string? EmailAddress { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("company")]
    public string? Company { get; set; }

    [JsonPropertyName("contact_list_key_value")]
    public IReadOnlyList<ContactKeyValue> ContactListKeyValue { get; set; } = Array.Empty<ContactKeyValue>();
}

public sealed class ContactKeyValue
{
    public string? Key { get; set; }

    public string? Value { get; set; }
}
