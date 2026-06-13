namespace Termii;

public sealed class GetSenderIdsRequest
{
    public string? SenderId { get; set; }

    public string? Status { get; set; }

    internal string ToPath()
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(SenderId))
        {
            query.Add($"sender_id={Uri.EscapeDataString(SenderId)}");
        }

        if (!string.IsNullOrWhiteSpace(Status))
        {
            query.Add($"status={Uri.EscapeDataString(Status)}");
        }

        return query.Count == 0
            ? "/api/sender-id"
            : $"/api/sender-id?{string.Join("&", query)}";
    }
}
