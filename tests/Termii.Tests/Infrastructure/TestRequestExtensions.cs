using System.Text.Json;

namespace Termii.Tests.Infrastructure;

internal static class TestRequestExtensions
{
    public static async Task<JsonDocument> ReadJsonBodyAsync(
        this HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        if (request.Content is null)
        {
            throw new InvalidOperationException("The request does not have a body.");
        }

        var json = await request.Content.ReadAsStringAsync(cancellationToken);

        return JsonDocument.Parse(json);
    }
}
