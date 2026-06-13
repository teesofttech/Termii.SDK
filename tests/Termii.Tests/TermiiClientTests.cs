using Termii;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
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
        using var handler = new RecordingHttpMessageHandler();
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test"),
        };
        var client = new TermiiClient(httpClient, new TermiiOptions
        {
            ApiKey = "test key",
            BaseUrl = new Uri("https://example.test"),
        });

        using var response = await client.SendAsync(HttpMethod.Get, "/api/sms/inbox", cancellationToken: CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpMethod.Get, handler.Request!.Method);
        Assert.Equal("https://example.test/api/sms/inbox?api_key=test%20key", handler.Request.RequestUri!.AbsoluteUri);
        Assert.Null(handler.Request.Content);
    }

    [Fact]
    public async Task SendAsyncAddsApiKeyToJsonBody()
    {
        using var handler = new RecordingHttpMessageHandler();
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test"),
        };
        var client = new TermiiClient(httpClient, new TermiiOptions
        {
            ApiKey = "test-api-key",
            BaseUrl = new Uri("https://example.test"),
        });

        using var response = await client.SendAsync(
            HttpMethod.Post,
            "/api/sms/send",
            new { to = "2348012345678", from = "Termii", sms = "Hello" },
            TermiiAuthenticationLocation.Body,
            CancellationToken.None);

        var json = await handler.Request!.Content!.ReadAsStringAsync(CancellationToken.None);
        using var document = JsonDocument.Parse(json);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpMethod.Post, handler.Request.Method);
        Assert.Equal("https://example.test/api/sms/send", handler.Request.RequestUri!.ToString());
        Assert.Equal("test-api-key", document.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("2348012345678", document.RootElement.GetProperty("to").GetString());
        Assert.Equal("Termii", document.RootElement.GetProperty("from").GetString());
        Assert.Equal("Hello", document.RootElement.GetProperty("sms").GetString());
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

internal sealed class RecordingHttpMessageHandler : HttpMessageHandler, IDisposable
{
    public HttpRequestMessage? Request { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Request = request;

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}"),
        });
    }
}
