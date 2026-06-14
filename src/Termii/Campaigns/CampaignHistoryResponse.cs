using System.Text.Json.Serialization;

namespace Termii;

public sealed class CampaignHistoryResponse
{
    public string? Id { get; set; }

    public int? ApplicationId { get; set; }

    public string? Uuid { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public string? CampaignId { get; set; }

    public string? PhonebookId { get; set; }

    public string? PhonebookName { get; set; }

    public string? Sender { get; set; }

    public string? Message { get; set; }

    public string? CountryCode { get; set; }

    public string? SmsType { get; set; }

    public string? CampaignType { get; set; }

    public string? Status { get; set; }

    public decimal? Cost { get; set; }

    public int? TotalRecipient { get; set; }

    public int? TotalDelivered { get; set; }

    public int? TotalFailed { get; set; }

    public int? Sent { get; set; }

    public string? RunAt { get; set; }

    public bool? IsLinkTrackingEnabled { get; set; }

    public bool? Rerun { get; set; }

    public string? SendBy { get; set; }

    public bool? Personalized { get; set; }

    [JsonExtensionData]
    public Dictionary<string, System.Text.Json.JsonElement>? AdditionalData { get; set; }
}
