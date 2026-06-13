using System.Net;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiNumberClientTests
{
    [Fact]
    public async Task SendAsyncPostsNumberMessageBodyAndDeserializesResponse()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "code": "ok",
              "balance": "38.75",
              "message_id": "number-12345",
              "message": "Successfully Sent",
              "user": "termii-user",
              "message_id_str": "number-12345"
            }
            """);
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Numbers.SendAsync(
            new SendNumberMessageRequest
            {
                To = "2348012345678",
                Sms = "Hello from the Number API",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/sms/number/send", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("2348012345678", body.RootElement.GetProperty("to").GetString());
        Assert.Equal("Hello from the Number API", body.RootElement.GetProperty("sms").GetString());
        Assert.False(body.RootElement.TryGetProperty("from", out _));

        Assert.Equal("ok", response.Code);
        Assert.Equal(38.75m, response.Balance);
        Assert.Equal("number-12345", response.MessageId);
        Assert.Equal("Successfully Sent", response.Message);
        Assert.Equal("termii-user", response.User);
        Assert.Equal("number-12345", response.MessageIdString);
    }

    [Fact]
    public async Task SendAsyncRejectsMissingRequiredFields()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Numbers.SendAsync(
            new SendNumberMessageRequest
            {
                To = "2348012345678",
                Sms = "",
            },
            CancellationToken.None));
    }
}
