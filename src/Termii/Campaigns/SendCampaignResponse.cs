using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendCampaignResponse
{
    public string? Message { get; set; }

    [JsonPropertyName("campaignId")]
    public string? CampaignId { get; set; }

    public string? Status { get; set; }
}
