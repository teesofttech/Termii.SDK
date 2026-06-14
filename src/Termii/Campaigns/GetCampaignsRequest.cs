namespace Termii;

public sealed class GetCampaignsRequest
{
    public int? Page { get; set; }

    public int? Size { get; set; }

    internal string ToPath()
    {
        var query = new List<string>();
        TermiiCampaignQueryString.AddIfPresent(query, "page", Page);
        TermiiCampaignQueryString.AddIfPresent(query, "size", Size);

        return TermiiCampaignQueryString.Append("/api/sms/campaigns", query);
    }
}
