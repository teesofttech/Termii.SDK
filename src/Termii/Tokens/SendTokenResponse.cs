using System.Text.Json.Serialization;

namespace Termii;

public sealed class SendTokenResponse
{
    [JsonPropertyName("smsStatus")]
    public string? SmsStatus { get; set; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("to")]
    public string? To { get; set; }

    [JsonPropertyName("pinId")]
    public string? PinId { get; set; }

    [JsonPropertyName("pin_id")]
    public string? PinIdSnakeCase { get; set; }

    [JsonPropertyName("message_id_str")]
    public string? MessageIdString { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
