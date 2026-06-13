using System.Net;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiMessagingClientTests
{
    [Fact]
    public async Task SendAsyncPostsMessageBodyAndDeserializesResponse()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "code": "ok",
              "balance": "42.50",
              "message_id": "12345",
              "message": "Successfully Sent",
              "user": "termii-user",
              "message_id_str": "12345"
            }
            """);
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Messaging.SendAsync(
            new SendMessageRequest
            {
                To = "2348012345678",
                From = "Termii",
                Sms = "Hello from the SDK",
                Type = TermiiMessageType.Unicode,
                Channel = TermiiMessageChannel.Dnd,
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/sms/send", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("2348012345678", body.RootElement.GetProperty("to").GetString());
        Assert.Equal("Termii", body.RootElement.GetProperty("from").GetString());
        Assert.Equal("Hello from the SDK", body.RootElement.GetProperty("sms").GetString());
        Assert.Equal("unicode", body.RootElement.GetProperty("type").GetString());
        Assert.Equal("dnd", body.RootElement.GetProperty("channel").GetString());

        Assert.Equal("ok", response.Code);
        Assert.Equal(42.50m, response.Balance);
        Assert.Equal("12345", response.MessageId);
        Assert.Equal("Successfully Sent", response.Message);
        Assert.Equal("termii-user", response.User);
        Assert.Equal("12345", response.MessageIdString);
    }

    [Fact]
    public async Task SendAsyncIncludesWhatsAppMedia()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await client.Messaging.SendAsync(
            new SendMessageRequest
            {
                To = "2348012345678",
                From = "Termii",
                Sms = "A document for you",
                Channel = TermiiMessageChannel.WhatsApp,
                Media = new TermiiMessageMedia
                {
                    Url = "https://example.test/file.pdf",
                    Caption = "Termii SDK",
                },
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);
        var media = body.RootElement.GetProperty("media");

        Assert.Equal("whatsapp", body.RootElement.GetProperty("channel").GetString());
        Assert.Equal("https://example.test/file.pdf", media.GetProperty("url").GetString());
        Assert.Equal("Termii SDK", media.GetProperty("caption").GetString());
    }

    [Fact]
    public async Task SendBulkAsyncPostsBulkMessageBody()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await client.Messaging.SendBulkAsync(
            new SendBulkMessageRequest
            {
                To = new[] { "2348012345678", "2348098765432" },
                From = "Termii",
                Sms = "Hello everyone",
                Channel = TermiiMessageChannel.Generic,
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/sms/send/bulk", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("2348012345678", body.RootElement.GetProperty("to")[0].GetString());
        Assert.Equal("2348098765432", body.RootElement.GetProperty("to")[1].GetString());
        Assert.Equal("generic", body.RootElement.GetProperty("channel").GetString());
        Assert.Equal("plain", body.RootElement.GetProperty("type").GetString());
    }

    [Fact]
    public async Task SendAsyncRejectsMissingRequiredFields()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Messaging.SendAsync(
            new SendMessageRequest
            {
                To = "2348012345678",
                From = "",
                Sms = "Hello",
            },
            CancellationToken.None));
    }

    [Fact]
    public async Task SendBulkAsyncRejectsMissingRecipients()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Messaging.SendBulkAsync(
            new SendBulkMessageRequest
            {
                To = Array.Empty<string>(),
                From = "Termii",
                Sms = "Hello",
            },
            CancellationToken.None));
    }
}
