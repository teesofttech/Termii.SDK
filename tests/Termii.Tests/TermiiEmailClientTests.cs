using System.Net;
using System.Text.Json;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiEmailClientTests
{
    [Fact]
    public async Task SendProductEmailAsyncPostsTemplateEmailBody()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"code":"ok","message":"Email accepted","message_id":"email-123","status":"queued"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Emails.SendProductEmailAsync(
            new SendProductEmailRequest
            {
                EmailAddress = "person@example.com",
                TemplateId = "template-123",
                Subject = "Order update",
                From = "noreply@example.com",
                SenderName = "Example Store",
                Data = new Dictionary<string, JsonElement>
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
        Assert.Equal("https://example.test/api/templates/send-email", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("person@example.com", body.RootElement.GetProperty("email_address").GetString());
        Assert.Equal("template-123", body.RootElement.GetProperty("template_id").GetString());
        Assert.Equal("Order update", body.RootElement.GetProperty("subject").GetString());
        Assert.Equal("noreply@example.com", body.RootElement.GetProperty("from").GetString());
        Assert.Equal("Example Store", body.RootElement.GetProperty("sender_name").GetString());
        Assert.Equal("Ada", body.RootElement.GetProperty("data").GetProperty("first_name").GetString());
        Assert.Equal("ORD-123", body.RootElement.GetProperty("data").GetProperty("order_id").GetString());

        Assert.Equal("ok", response.Code);
        Assert.Equal("Email accepted", response.Message);
        Assert.Equal("email-123", response.MessageId);
        Assert.Equal("queued", response.Status);
    }

    [Fact]
    public async Task SendProductEmailAsyncRejectsMissingRequiredFields()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Emails.SendProductEmailAsync(
            new SendProductEmailRequest
            {
                EmailAddress = "",
                TemplateId = "template-123",
            },
            CancellationToken.None));
    }
}
