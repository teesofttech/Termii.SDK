namespace Termii;

public sealed class CheckDndRequest
{
    public string PhoneNumber { get; set; } = string.Empty;

    internal string ToPath()
    {
        TermiiRequestValidation.Required(PhoneNumber, nameof(PhoneNumber));

        var query = new List<string>();
        TermiiInsightsQueryString.AddIfPresent(query, "phone_number", PhoneNumber);

        return TermiiInsightsQueryString.Append("/api/check/dnd", query);
    }
}
