namespace Termii;

public sealed class GetMessageHistoryRequest
{
    public int? Page { get; set; }

    public int? Size { get; set; }

    public string? Sender { get; set; }

    public string? Receiver { get; set; }

    public string? MessageId { get; set; }

    public string? Status { get; set; }

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    internal string ToPath()
    {
        var query = new List<string>();
        TermiiInsightsQueryString.AddIfPresent(query, "page", Page);
        TermiiInsightsQueryString.AddIfPresent(query, "size", Size);
        TermiiInsightsQueryString.AddIfPresent(query, "sender", Sender);
        TermiiInsightsQueryString.AddIfPresent(query, "receiver", Receiver);
        TermiiInsightsQueryString.AddIfPresent(query, "message_id", MessageId);
        TermiiInsightsQueryString.AddIfPresent(query, "status", Status);
        TermiiInsightsQueryString.AddIfPresent(query, "from", FromDate);
        TermiiInsightsQueryString.AddIfPresent(query, "to", ToDate);

        return TermiiInsightsQueryString.Append("/api/sms/inbox", query);
    }
}
