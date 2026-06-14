using System.Text.Json.Serialization;

namespace Termii;

public sealed class CampaignListResponse
{
    public IReadOnlyList<CampaignRecord> Content { get; set; } = Array.Empty<CampaignRecord>();

    public int? TotalPages { get; set; }

    public int? TotalElements { get; set; }

    public int? Size { get; set; }

    public int? Number { get; set; }

    public bool? First { get; set; }

    public bool? Last { get; set; }

    public bool? Empty { get; set; }
}

public sealed class CampaignRecord
{
    [JsonPropertyName("campaign_id")]
    public string? CampaignId { get; set; }

    [JsonPropertyName("run_at")]
    public string? RunAt { get; set; }

    public string? Status { get; set; }

    [JsonPropertyName("created_at")]
    public long? CreatedAt { get; set; }

    [JsonPropertyName("phone_book")]
    public string? Phonebook { get; set; }

    [JsonPropertyName("camp_type")]
    public string? CampaignType { get; set; }

    [JsonPropertyName("total_recipients")]
    public int? TotalRecipients { get; set; }
}
