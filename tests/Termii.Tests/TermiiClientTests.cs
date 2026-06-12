using Termii;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiClientTests
{
    [Fact]
    public void ConstructorRejectsMissingApiKey()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => new TermiiClient(new TermiiOptions()));

        Assert.Contains("API key", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ConstructorAcceptsValidOptions()
    {
        var options = new TermiiOptions
        {
            ApiKey = "test-api-key",
        };

        var client = new TermiiClient(options);

        Assert.Equal("test-api-key", client.Options.ApiKey);
        Assert.Equal(TermiiOptions.DefaultBaseUrl, client.Options.BaseUrl.ToString().TrimEnd('/'));
    }
}
