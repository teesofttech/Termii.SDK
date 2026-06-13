using Termii;
using Termii.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
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

    [Fact]
    public async Task SendAsyncAddsApiKeyToQueryString()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler, apiKey: "test key");

        using var response = await client.SendAsync(HttpMethod.Get, "/api/sms/inbox", cancellationToken: CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Equal("https://example.test/api/sms/inbox?api_key=test%20key", handler.LastRequest.RequestUri!.AbsoluteUri);
        Assert.Null(handler.LastRequest.Content);
    }

    [Fact]
    public async Task SendAsyncAddsApiKeyToJsonBody()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        using var response = await client.SendAsync(
            HttpMethod.Post,
            "/api/sms/send",
            new { to = "2348012345678", from = "Termii", sms = "Hello" },
            TermiiAuthenticationLocation.Body,
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var document = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/sms/send", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", document.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("2348012345678", document.RootElement.GetProperty("to").GetString());
        Assert.Equal("Termii", document.RootElement.GetProperty("from").GetString());
        Assert.Equal("Hello", document.RootElement.GetProperty("sms").GetString());
    }

    [Fact]
    public async Task SendAsyncThrowsTermiiApiExceptionForJsonErrorResponses()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.BadRequest,
            """{"message":"Invalid sender ID","code":"sender_id_invalid"}""");
        var client = TestTermiiClientFactory.Create(handler, apiKey: "secret-api-key");

        var exception = await Assert.ThrowsAsync<TermiiApiException>(() => client.SendAsync(
            HttpMethod.Post,
            "/api/sms/send",
            new { to = "2348012345678", from = "Termii", sms = "Hello" },
            TermiiAuthenticationLocation.Body,
            CancellationToken.None));

        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
        Assert.Equal("Invalid sender ID", exception.TermiiMessage);
        Assert.Equal("sender_id_invalid", exception.TermiiCode);
        Assert.Equal("""{"message":"Invalid sender ID","code":"sender_id_invalid"}""", exception.RawResponseBody);
        Assert.DoesNotContain("secret-api-key", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task SendAsyncThrowsTermiiApiExceptionForPlainTextErrorResponses()
    {
        using var handler = new TestHttpMessageHandler(HttpStatusCode.InternalServerError, "Service unavailable");
        var client = TestTermiiClientFactory.Create(handler, apiKey: "secret-api-key");

        var exception = await Assert.ThrowsAsync<TermiiApiException>(() => client.SendAsync(
            HttpMethod.Get,
            "/api/get-balance",
            cancellationToken: CancellationToken.None));

        Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
        Assert.Null(exception.TermiiMessage);
        Assert.Null(exception.TermiiCode);
        Assert.Equal("Service unavailable", exception.RawResponseBody);
        Assert.DoesNotContain("secret-api-key", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("api/sms/send")]
    [InlineData("https://example.test/api/sms/send")]
    public async Task SendAsyncRejectsInvalidPaths(string path)
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.SendAsync(
            HttpMethod.Get,
            path,
            cancellationToken: CancellationToken.None));
    }

    [Fact]
    public void AddTermiiRegistersConfiguredClient()
    {
        var services = new ServiceCollection();

        services.AddTermii(options =>
        {
            options.ApiKey = "test-api-key";
            options.BaseUrl = new Uri("https://example.test");
            options.Timeout = TimeSpan.FromSeconds(30);
        });

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<TermiiClient>();

        Assert.Equal("test-api-key", client.Options.ApiKey);
        Assert.Equal("https://example.test/", client.Options.BaseUrl.ToString());
        Assert.Equal(TimeSpan.FromSeconds(30), client.Options.Timeout);
    }
}
