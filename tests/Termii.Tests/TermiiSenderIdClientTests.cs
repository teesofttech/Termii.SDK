using System.Net;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiSenderIdClientTests
{
    [Fact]
    public async Task GetAsyncAddsApiKeyAndDeserializesSenderIds()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "content": [
                {
                  "sender_id": "Termii",
                  "status": "active",
                  "country": "NG",
                  "company": "Teesoft Tech",
                  "usecase": "Transactional alerts",
                  "createdAt": "2026-06-13 10:00:00"
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

        var response = await client.SenderIds.GetAsync(
            new GetSenderIdsRequest
            {
                SenderId = "Termii",
                Status = "active",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        var senderId = Assert.Single(response.Content);

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal(
            "https://example.test/api/sender-id?sender_id=Termii&status=active&api_key=test-api-key",
            request.RequestUri!.AbsoluteUri);
        Assert.Null(request.Content);
        Assert.Equal(1, response.TotalElements);
        Assert.Equal("Termii", senderId.SenderId);
        Assert.Equal("active", senderId.Status);
        Assert.Equal("NG", senderId.Country);
        Assert.Equal("Teesoft Tech", senderId.Company);
        Assert.Equal("Transactional alerts", senderId.UseCase);
        Assert.Equal("2026-06-13 10:00:00", senderId.CreatedAt);
    }

    [Fact]
    public async Task RequestAsyncPostsRequestBodyAndDeserializesResponse()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"code":"ok","message":"Sender ID request received"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.SenderIds.RequestAsync(
            new RequestSenderIdRequest
            {
                SenderId = "Termii",
                UseCase = "Transactional alerts",
                Company = "Teesoft Tech",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/sender-id/request", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("Termii", body.RootElement.GetProperty("sender_id").GetString());
        Assert.Equal("Transactional alerts", body.RootElement.GetProperty("use_case").GetString());
        Assert.Equal("Teesoft Tech", body.RootElement.GetProperty("company").GetString());
        Assert.Equal("ok", response.Code);
        Assert.Equal("Sender ID request received", response.Message);
    }

    [Fact]
    public async Task RequestAsyncRejectsMissingRequiredFields()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.SenderIds.RequestAsync(
            new RequestSenderIdRequest
            {
                SenderId = "",
                UseCase = "Transactional alerts",
                Company = "Teesoft Tech",
            },
            CancellationToken.None));
    }
}
