using System.Text.Json;

namespace Termii;

internal sealed class TermiiApiError
{
    public string? Message { get; private set; }

    public string? Code { get; private set; }

    public static TermiiApiError Parse(string rawResponseBody)
    {
        if (string.IsNullOrWhiteSpace(rawResponseBody))
        {
            return new TermiiApiError();
        }

        try
        {
            using var document = JsonDocument.Parse(rawResponseBody);

            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                return new TermiiApiError();
            }

            return new TermiiApiError
            {
                Message = ReadString(document.RootElement, "message")
                    ?? ReadString(document.RootElement, "error")
                    ?? ReadString(document.RootElement, "errors")
                    ?? ReadString(document.RootElement, "status"),
                Code = ReadString(document.RootElement, "code")
                    ?? ReadString(document.RootElement, "error_code"),
            };
        }
        catch (JsonException)
        {
            return new TermiiApiError();
        }
    }

    private static string? ReadString(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property))
        {
            return null;
        }

        return property.ValueKind switch
        {
            JsonValueKind.String => property.GetString(),
            JsonValueKind.Number => property.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            _ => property.GetRawText(),
        };
    }
}
