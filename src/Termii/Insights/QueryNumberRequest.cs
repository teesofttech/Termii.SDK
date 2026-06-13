namespace Termii;

public sealed class QueryNumberRequest
{
    public string PhoneNumber { get; set; } = string.Empty;

    public string? CountryCode { get; set; }

    internal string ToPath()
    {
        TermiiRequestValidation.Required(PhoneNumber, nameof(PhoneNumber));

        var query = new List<string>();
        TermiiInsightsQueryString.AddIfPresent(query, "phone_number", PhoneNumber);
        TermiiInsightsQueryString.AddIfPresent(query, "country_code", CountryCode);

        return TermiiInsightsQueryString.Append("/api/insight/number/query", query);
    }
}
