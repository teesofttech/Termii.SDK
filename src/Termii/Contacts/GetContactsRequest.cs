namespace Termii;

public sealed class GetContactsRequest
{
    public int? Page { get; set; }

    public int? Size { get; set; }

    internal string ToPath(string phonebookId)
    {
        TermiiRequestValidation.Required(phonebookId, nameof(phonebookId));

        var query = new List<string>();
        TermiiCampaignQueryString.AddIfPresent(query, "page", Page);
        TermiiCampaignQueryString.AddIfPresent(query, "size", Size);

        return TermiiCampaignQueryString.Append(
            $"/api/phonebooks/{Uri.EscapeDataString(phonebookId)}/contacts",
            query);
    }
}
