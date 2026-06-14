using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendCampaignRequest
{
    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; } = string.Empty;

    [JsonPropertyName("sender_id")]
    public string SenderId { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("channel")]
    public string Channel { get; set; } = string.Empty;

    [JsonPropertyName("message_type")]
    public string MessageType { get; set; } = string.Empty;

    [JsonPropertyName("phonebook_id")]
    public string PhonebookId { get; set; } = string.Empty;

    [JsonPropertyName("enable_link_tracking")]
    public bool? EnableLinkTracking { get; set; }

    [JsonPropertyName("campaign_type")]
    public string CampaignType { get; set; } = string.Empty;

    [JsonPropertyName("schedule_sms_status")]
    public string ScheduleSmsStatus { get; set; } = string.Empty;

    [JsonPropertyName("schedule_time")]
    public string? ScheduleTime { get; set; }

    [JsonPropertyName("delimiter")]
    public string? Delimiter { get; set; }

    [JsonPropertyName("remove_duplicate")]
    public string? RemoveDuplicate { get; set; }

    internal void Validate()
    {
        TermiiRequestValidation.Required(CountryCode, nameof(CountryCode));
        TermiiRequestValidation.Required(SenderId, nameof(SenderId));
        TermiiRequestValidation.Required(Message, nameof(Message));
        TermiiRequestValidation.Required(Channel, nameof(Channel));
        TermiiRequestValidation.Required(MessageType, nameof(MessageType));
        TermiiRequestValidation.Required(PhonebookId, nameof(PhonebookId));
        TermiiRequestValidation.Required(CampaignType, nameof(CampaignType));
        TermiiRequestValidation.Required(ScheduleSmsStatus, nameof(ScheduleSmsStatus));
    }
}
