namespace Termii;

internal static class TermiiInsightsQueryString
{
    public static void AddIfPresent(List<string> query, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            query.Add($"{name}={Uri.EscapeDataString(value)}");
        }
    }

    public static void AddIfPresent(List<string> query, string name, int? value)
    {
        if (value.HasValue)
        {
            query.Add($"{name}={value.Value}");
        }
    }

    public static string Append(string path, List<string> query)
    {
        return query.Count == 0
            ? path
            : $"{path}?{string.Join("&", query)}";
    }
}
