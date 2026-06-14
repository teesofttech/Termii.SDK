using System.Text.Json;
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
}
