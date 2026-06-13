using System.Text.Json.Serialization;

namespace Termii;

public sealed class RequestSenderIdResponse
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
