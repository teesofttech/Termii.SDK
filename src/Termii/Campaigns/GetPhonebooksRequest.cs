namespace Termii;

public sealed class GetPhonebooksRequest
{
    public int? Page { get; set; }

    public int? Size { get; set; }

    public string? Name { get; set; }

    internal string ToPath()
    {
        var query = new List<string>();
        TermiiCampaignQueryString.AddIfPresent(query, "page", Page);
        TermiiCampaignQueryString.AddIfPresent(query, "size", Size);
        TermiiCampaignQueryString.AddIfPresent(query, "name", Name);

        return TermiiCampaignQueryString.Append("/api/phonebooks", query);
    }
}
