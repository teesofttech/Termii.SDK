using Termii;
using Xunit;

namespace Termii.IntegrationTests;

public sealed class TermiiClientIntegrationTests
{
    [Fact]
    public void CanCreateClientFromIntegrationEnvironment()
    {
        var apiKey = Environment.GetEnvironmentVariable("TERMII_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return;
        }

        var baseUrl = Environment.GetEnvironmentVariable("TERMII_BASE_URL");
        var options = new TermiiOptions
        {
            ApiKey = apiKey,
        };

        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            options.BaseUrl = new Uri(baseUrl);
        }

        var client = new TermiiClient(options);

        Assert.False(string.IsNullOrWhiteSpace(client.Options.ApiKey));
        Assert.True(client.Options.BaseUrl.IsAbsoluteUri);
    }
}
