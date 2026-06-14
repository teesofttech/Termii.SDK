using System.Net;
using System.Text.Json;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiWhatsAppTemplateClientTests
{
    [Fact]
    public async Task SendWhatsAppTemplateAsyncPostsTemplateBody()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"code":"ok","message":"Template accepted","message_id":"template-123","status":"queued"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Messaging.SendWhatsAppTemplateAsync(
            new SendWhatsAppTemplateRequest
            {
                PhoneNumber = "2348012345678",
                DeviceId = "device-123",
                TemplateId = "template-123",
                TemplateName = "order_update",
                Language = "en",
                Variables = new Dictionary<string, JsonElement>
                {
                    ["first_name"] = JsonDocument.Parse("\"Ada\"").RootElement.Clone(),
                    ["order_id"] = JsonDocument.Parse("\"ORD-123\"").RootElement.Clone(),
                },
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/send/template", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("2348012345678", body.RootElement.GetProperty("phone_number").GetString());
        Assert.Equal("device-123", body.RootElement.GetProperty("device_id").GetString());
        Assert.Equal("template-123", body.RootElement.GetProperty("template_id").GetString());
        Assert.Equal("order_update", body.RootElement.GetProperty("template_name").GetString());
        Assert.Equal("en", body.RootElement.GetProperty("language").GetString());
        Assert.Equal("Ada", body.RootElement.GetProperty("variables").GetProperty("first_name").GetString());

        Assert.Equal("ok", response.Code);
        Assert.Equal("Template accepted", response.Message);
        Assert.Equal("template-123", response.MessageId);
        Assert.Equal("queued", response.Status);
    }

    [Fact]
    public async Task SendWhatsAppTemplateMediaAsyncPostsMediaTemplateBody()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"message":"Media template accepted","status":"queued"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Messaging.SendWhatsAppTemplateMediaAsync(
            new SendWhatsAppTemplateMediaRequest
            {
                PhoneNumber = "2348012345678",
                DeviceId = "device-123",
                TemplateId = "template-media-123",
                Language = "en",
                Media = new WhatsAppTemplateMedia
                {
                    Type = "document",
                    Url = "https://example.test/invoice.pdf",
                    Caption = "Invoice",
                    FileName = "invoice.pdf",
                },
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);
        var media = body.RootElement.GetProperty("media");

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/send/template/media", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("template-media-123", body.RootElement.GetProperty("template_id").GetString());
        Assert.Equal("document", media.GetProperty("type").GetString());
        Assert.Equal("https://example.test/invoice.pdf", media.GetProperty("url").GetString());
        Assert.Equal("Invoice", media.GetProperty("caption").GetString());
        Assert.Equal("invoice.pdf", media.GetProperty("filename").GetString());
        Assert.Equal("Media template accepted", response.Message);
    }

    [Fact]
    public async Task SendWhatsAppTemplateAsyncRejectsMissingRequiredFields()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Messaging.SendWhatsAppTemplateAsync(
            new SendWhatsAppTemplateRequest
            {
                PhoneNumber = "",
                DeviceId = "device-123",
                TemplateId = "template-123",
            },
            CancellationToken.None));
    }

    [Fact]
    public async Task SendWhatsAppTemplateMediaAsyncRejectsMissingMediaUrl()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Messaging.SendWhatsAppTemplateMediaAsync(
            new SendWhatsAppTemplateMediaRequest
            {
                PhoneNumber = "2348012345678",
                DeviceId = "device-123",
                TemplateId = "template-123",
                Media = new WhatsAppTemplateMedia
                {
                    Type = "image",
                    Url = "",
                },
            },
            CancellationToken.None));
    }
}
