using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

public sealed class QueryNumberResponse
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("network")]
    public string? Network { get; set; }

    [JsonPropertyName("network_code")]
    public string? NetworkCode { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("country_name")]
    public string? CountryName { get; set; }

    [JsonPropertyName("ported")]
    public bool? Ported { get; set; }

    [JsonPropertyName("roaming")]
    public bool? Roaming { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalData { get; set; }
}
