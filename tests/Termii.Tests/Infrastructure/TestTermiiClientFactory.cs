using Termii;

namespace Termii.Tests.Infrastructure;

internal static class TestTermiiClientFactory
{
    public static TermiiClient Create(
        TestHttpMessageHandler handler,
        string apiKey = "test-api-key",
        string baseUrl = "https://example.test")
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl),
        };

        return new TermiiClient(httpClient, new TermiiOptions
        {
            ApiKey = apiKey,
            BaseUrl = new Uri(baseUrl),
        });
    }
}
