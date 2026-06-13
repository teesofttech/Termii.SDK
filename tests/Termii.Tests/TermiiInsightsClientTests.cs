using System.Net;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiInsightsClientTests
{
    [Fact]
    public async Task GetBalanceAsyncAddsApiKeyAndDeserializesBalance()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"user":"termii-user","balance":"125.50","currency":"NGN"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Insights.GetBalanceAsync(CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("https://example.test/api/get-balance?api_key=test-api-key", request.RequestUri!.AbsoluteUri);
        Assert.Null(request.Content);
        Assert.Equal("termii-user", response.User);
        Assert.Equal(125.50m, response.Balance);
        Assert.Equal("NGN", response.Currency);
    }

    [Fact]
    public async Task CheckDndAsyncAddsPhoneNumberAndApiKey()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"phone_number":"2348012345678","status":"DND","network":"MTN","dnd_active":true}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Insights.CheckDndAsync(
            new CheckDndRequest
            {
                PhoneNumber = "2348012345678",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);

        Assert.Equal(
            "https://example.test/api/check/dnd?phone_number=2348012345678&api_key=test-api-key",
            request.RequestUri!.AbsoluteUri);
        Assert.Equal("2348012345678", response.PhoneNumber);
        Assert.Equal("DND", response.Status);
        Assert.Equal("MTN", response.Network);
        Assert.True(response.DndActive);
    }

    [Fact]
    public async Task QueryNumberAsyncAddsCountryCodeAndDeserializesStatus()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"number":"2348012345678","status":"active","network":"MTN","country_code":"NG","country_name":"Nigeria","ported":false}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Insights.QueryNumberAsync(
            new QueryNumberRequest
            {
                PhoneNumber = "2348012345678",
                CountryCode = "NG",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);

        Assert.Equal(
            "https://example.test/api/insight/number/query?phone_number=2348012345678&country_code=NG&api_key=test-api-key",
            request.RequestUri!.AbsoluteUri);
        Assert.Equal("active", response.Status);
        Assert.Equal("MTN", response.Network);
        Assert.Equal("Nigeria", response.CountryName);
        Assert.False(response.Ported);
    }

    [Fact]
    public async Task GetMessageHistoryAsyncAddsFiltersAndDeserializesContent()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "content": [
                {
                  "message_id": "msg-123",
                  "sender": "Termii",
                  "receiver": "2348012345678",
                  "message": "Hello",
                  "amount": "4.25",
                  "reroute": 0,
                  "status": "sent",
                  "sms_type": "plain",
                  "send_by": "api",
                  "created_at": "2026-06-13 10:00:00",
                  "updated_at": "2026-06-13 10:01:00"
                }
              ],
              "totalElements": 1,
              "totalPages": 1,
              "size": 15,
              "number": 0,
              "first": true,
              "last": true,
              "empty": false
            }
            """);
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Insights.GetMessageHistoryAsync(
            new GetMessageHistoryRequest
            {
                Page = 0,
                Size = 15,
                Sender = "Termii",
                Receiver = "2348012345678",
                Status = "sent",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        var message = Assert.Single(response.Content);

        Assert.Equal(
            "https://example.test/api/sms/inbox?page=0&size=15&sender=Termii&receiver=2348012345678&status=sent&api_key=test-api-key",
            request.RequestUri!.AbsoluteUri);
        Assert.Equal(1, response.TotalElements);
        Assert.Equal("msg-123", message.MessageId);
        Assert.Equal("Termii", message.Sender);
        Assert.Equal("2348012345678", message.Receiver);
        Assert.Equal(4.25m, message.Amount);
        Assert.Equal("sent", message.Status);
    }

    [Fact]
    public async Task CheckDndAsyncRejectsMissingPhoneNumber()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Insights.CheckDndAsync(
            new CheckDndRequest
            {
                PhoneNumber = "",
            },
            CancellationToken.None));
    }
}
