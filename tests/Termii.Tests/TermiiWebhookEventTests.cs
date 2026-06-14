using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using Termii;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiWebhookEventTests
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
    };

    [Fact]
    public void CanDeserializeDeliveryReportPayload()
    {
        var webhookEvent = JsonSerializer.Deserialize<TermiiWebhookEvent>(
            """
            {
              "event": "message.status",
              "message_id": "msg-123",
              "message_id_str": "msg-123",
              "status": "delivered",
              "to": "2348012345678",
              "from": "Termii",
              "channel": "generic",
              "network": "MTN",
              "amount": "4.25",
              "delivered_at": "2026-06-14 09:00:00",
              "provider_reference": "provider-123"
            }
            """,
            JsonSerializerOptions);

        Assert.NotNull(webhookEvent);
        Assert.Equal("message.status", webhookEvent.Event);
        Assert.Equal("msg-123", webhookEvent.MessageId);
        Assert.Equal("delivered", webhookEvent.Status);
        Assert.Equal("2348012345678", webhookEvent.To);
        Assert.Equal("Termii", webhookEvent.From);
        Assert.Equal("generic", webhookEvent.Channel);
        Assert.Equal("MTN", webhookEvent.Network);
        Assert.Equal(4.25m, webhookEvent.Amount);
        Assert.Equal("2026-06-14 09:00:00", webhookEvent.DeliveredAt);
        Assert.NotNull(webhookEvent.AdditionalData);
        Assert.Equal("provider-123", webhookEvent.AdditionalData["provider_reference"].GetString());
    }

    [Fact]
    public void CanDeserializeFailedReportPayload()
    {
        var webhookEvent = JsonSerializer.Deserialize<TermiiWebhookEvent>(
            """
            {
              "type": "delivery_report",
              "message_id": "msg-456",
              "status": "failed",
              "receiver": "2348012345678",
              "sender": "Termii",
              "error_code": "3001",
              "error_message": "Insufficient balance",
              "done_date": "2026-06-14 09:05:00"
            }
            """,
            JsonSerializerOptions);

        Assert.NotNull(webhookEvent);
        Assert.Equal("delivery_report", webhookEvent.Type);
        Assert.Equal("msg-456", webhookEvent.MessageId);
        Assert.Equal("failed", webhookEvent.Status);
        Assert.Equal("2348012345678", webhookEvent.Receiver);
        Assert.Equal("Termii", webhookEvent.Sender);
        Assert.Equal("3001", webhookEvent.ErrorCode);
        Assert.Equal("Insufficient balance", webhookEvent.ErrorMessage);
        Assert.Equal("2026-06-14 09:05:00", webhookEvent.DoneDate);
    }

    [Fact]
    public void VerifyAcceptsValidSignature()
    {
        const string payload = """{"type":"delivery_report","message_id":"msg-123"}""";
        const string secretKey = "webhook-secret";
        var signature = Sign(payload, secretKey);

        var verified = TermiiWebhookSignature.Verify(payload, signature, secretKey);

        Assert.True(verified);
    }

    [Fact]
    public void VerifyAcceptsSha512PrefixedSignature()
    {
        const string payload = """{"type":"delivery_report","message_id":"msg-123"}""";
        const string secretKey = "webhook-secret";
        var signature = $"sha512={Sign(payload, secretKey)}";

        var verified = TermiiWebhookSignature.Verify(payload, signature, secretKey);

        Assert.True(verified);
    }

    [Fact]
    public void VerifyRejectsAlteredPayload()
    {
        const string payload = """{"type":"delivery_report","message_id":"msg-123"}""";
        const string secretKey = "webhook-secret";
        var signature = Sign(payload, secretKey);

        var verified = TermiiWebhookSignature.Verify(
            """{"type":"delivery_report","message_id":"msg-456"}""",
            signature,
            secretKey);

        Assert.False(verified);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-a-signature")]
    [InlineData("sha512=not-a-signature")]
    public void VerifyRejectsMissingOrMalformedSignature(string? signature)
    {
        var verified = TermiiWebhookSignature.Verify(
            """{"type":"delivery_report"}""",
            signature,
            "webhook-secret");

        Assert.False(verified);
    }

    [Fact]
    public void VerifyAcceptsRawPayloadBytes()
    {
        const string payload = """{"type":"delivery_report","message_id":"msg-123"}""";
        const string secretKey = "webhook-secret";
        var signature = Sign(payload, secretKey);

        var verified = TermiiWebhookSignature.Verify(
            Encoding.UTF8.GetBytes(payload),
            signature,
            secretKey);

        Assert.True(verified);
    }

    private static string Sign(string payload, string secretKey)
    {
        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        var builder = new StringBuilder(hash.Length * 2);

        foreach (var value in hash)
        {
            builder.Append(value.ToString("x2"));
        }

        return builder.ToString();
    }
}
