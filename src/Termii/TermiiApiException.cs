using System.Net;

namespace Termii;

public sealed class TermiiApiException : Exception
{
    public TermiiApiException(
        HttpStatusCode statusCode,
        string? termiiMessage,
        string? termiiCode,
        string rawResponseBody)
        : base(CreateMessage(statusCode, termiiMessage, termiiCode))
    {
        StatusCode = statusCode;
        TermiiMessage = termiiMessage;
        TermiiCode = termiiCode;
        RawResponseBody = rawResponseBody;
    }

    public HttpStatusCode StatusCode { get; }

    public string? TermiiMessage { get; }

    public string? TermiiCode { get; }

    public string RawResponseBody { get; }

    private static string CreateMessage(HttpStatusCode statusCode, string? termiiMessage, string? termiiCode)
    {
        var message = $"Termii API request failed with HTTP {(int)statusCode} ({statusCode}).";

        if (!string.IsNullOrWhiteSpace(termiiCode))
        {
            message += $" Code: {termiiCode}.";
        }

        if (!string.IsNullOrWhiteSpace(termiiMessage))
        {
            message += $" Message: {termiiMessage}.";
        }

        return message;
    }
}
