namespace Termii;

public sealed class GetMessageAnalyticsRequest
{
    public string? MessageId { get; set; }

    public string? DateFrom { get; set; }

    public string? DateTo { get; set; }

    public string? PhoneNumber { get; set; }

    internal string ToPath()
    {
        var query = new List<string>();
        TermiiInsightsQueryString.AddIfPresent(query, "message_id", MessageId);
        TermiiInsightsQueryString.AddIfPresent(query, "date_from", DateFrom);
        TermiiInsightsQueryString.AddIfPresent(query, "date_to", DateTo);
        TermiiInsightsQueryString.AddIfPresent(query, "phone_number", PhoneNumber);

        return TermiiInsightsQueryString.Append("/api/sms/history/analytics", query);
    }
}
