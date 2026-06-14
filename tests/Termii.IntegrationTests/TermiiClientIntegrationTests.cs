using Termii;
using Xunit;

namespace Termii.IntegrationTests;

public sealed class TermiiClientIntegrationTests
{
    [Fact]
    public void CanCreateClientFromIntegrationEnvironment()
    {
        if (!IntegrationTestEnvironment.HasCredentials)
        {
            return;
        }

        var client = new TermiiClient(IntegrationTestEnvironment.CreateOptions());

        Assert.False(string.IsNullOrWhiteSpace(client.Options.ApiKey));
        Assert.True(client.Options.BaseUrl.IsAbsoluteUri);
        Assert.NotNull(client.Messaging);
        Assert.NotNull(client.SenderIds);
        Assert.NotNull(client.Numbers);
        Assert.NotNull(client.Tokens);
        Assert.NotNull(client.Insights);
        Assert.NotNull(client.Campaigns);
        Assert.NotNull(client.Contacts);
        Assert.NotNull(client.Emails);
    }
}
