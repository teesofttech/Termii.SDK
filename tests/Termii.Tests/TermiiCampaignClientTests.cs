using System.Net;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiCampaignClientTests
{
    [Fact]
    public async Task GetPhonebooksAsyncAddsFiltersAndDeserializesResponse()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "content": [
                {
                  "phonebook_id": "pb-123",
                  "phonebook_name": "Customers",
                  "description": "Customer contacts",
                  "total_contacts": 42,
                  "created_at": "2026-06-14 10:00:00"
                }
              ],
              "totalElements": 1,
              "totalPages": 1,
              "size": 15,
              "number": 0
            }
            """);
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Campaigns.GetPhonebooksAsync(
            new GetPhonebooksRequest
            {
                Page = 0,
                Size = 15,
                Name = "Customers",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        var phonebook = Assert.Single(response.Content);

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal(
            "https://example.test/api/phonebooks?page=0&size=15&name=Customers&api_key=test-api-key",
            request.RequestUri!.AbsoluteUri);
        Assert.Null(request.Content);
        Assert.Equal(1, response.TotalElements);
        Assert.Equal("pb-123", phonebook.PhonebookId);
        Assert.Equal("Customers", phonebook.PhonebookName);
        Assert.Equal(42, phonebook.TotalContacts);
    }

    [Fact]
    public async Task CreatePhonebookAsyncPostsJsonBody()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"code":"ok","message":"Phonebook created","data":{"phonebook_id":"pb-123","phonebook_name":"Customers"}}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Campaigns.CreatePhonebookAsync(
            new CreatePhonebookRequest
            {
                PhonebookName = "Customers",
                Description = "Customer contacts",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/phonebooks", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("Customers", body.RootElement.GetProperty("phonebook_name").GetString());
        Assert.Equal("Customer contacts", body.RootElement.GetProperty("description").GetString());
        Assert.Equal("ok", response.Code);
        Assert.Equal("pb-123", response.Data!.PhonebookId);
    }

    [Fact]
    public async Task UpdatePhonebookAsyncPatchesEscapedPhonebookPath()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"message":"Phonebook updated","phonebook":{"id":"pb 123","name":"Customers 2026"}}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Campaigns.UpdatePhonebookAsync(
            "pb 123",
            new UpdatePhonebookRequest
            {
                PhonebookName = "Customers 2026",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(new HttpMethod("PATCH"), request.Method);
        Assert.Equal("https://example.test/api/phonebooks/pb%20123", request.RequestUri!.AbsoluteUri);
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("Customers 2026", body.RootElement.GetProperty("phonebook_name").GetString());
        Assert.Equal("Phonebook updated", response.Message);
        Assert.Equal("Customers 2026", response.Phonebook!.Name);
    }

    [Fact]
    public async Task DeletePhonebookAsyncAddsApiKeyToQueryString()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"status":"success","message":"Phonebook deleted"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Campaigns.DeletePhonebookAsync("pb-123", CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);

        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal("https://example.test/api/phonebooks/pb-123?api_key=test-api-key", request.RequestUri!.AbsoluteUri);
        Assert.Null(request.Content);
        Assert.Equal("success", response.Status);
        Assert.Equal("Phonebook deleted", response.Message);
    }

    [Fact]
    public async Task CreatePhonebookAsyncRejectsMissingName()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Campaigns.CreatePhonebookAsync(
            new CreatePhonebookRequest
            {
                PhonebookName = "",
            },
            CancellationToken.None));
    }
}
