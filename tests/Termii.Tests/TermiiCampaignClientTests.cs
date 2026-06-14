using System.Net;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiCampaignClientTests
{
    [Fact]
    public async Task SendAsyncPostsCampaignBody()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.Created,
            """{"message":"Your campaign has been scheduled","campaignId":"C714360330258","status":"success"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Campaigns.SendAsync(
            new SendCampaignRequest
            {
                CountryCode = "234",
                SenderId = "Termii",
                Message = "Welcome to Termii.",
                Channel = "generic",
                MessageType = "Plain",
                PhonebookId = "pb-123",
                Delimiter = ",",
                RemoveDuplicate = "yes",
                EnableLinkTracking = true,
                CampaignType = "personalized",
                ScheduleTime = "30-06-2026 6:00",
                ScheduleSmsStatus = "scheduled",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/sms/campaigns/send", request.RequestUri!.AbsoluteUri);
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("234", body.RootElement.GetProperty("country_code").GetString());
        Assert.Equal("Termii", body.RootElement.GetProperty("sender_id").GetString());
        Assert.Equal("Welcome to Termii.", body.RootElement.GetProperty("message").GetString());
        Assert.Equal("generic", body.RootElement.GetProperty("channel").GetString());
        Assert.Equal("Plain", body.RootElement.GetProperty("message_type").GetString());
        Assert.Equal("pb-123", body.RootElement.GetProperty("phonebook_id").GetString());
        Assert.Equal(",", body.RootElement.GetProperty("delimiter").GetString());
        Assert.Equal("yes", body.RootElement.GetProperty("remove_duplicate").GetString());
        Assert.True(body.RootElement.GetProperty("enable_link_tracking").GetBoolean());
        Assert.Equal("personalized", body.RootElement.GetProperty("campaign_type").GetString());
        Assert.Equal("30-06-2026 6:00", body.RootElement.GetProperty("schedule_time").GetString());
        Assert.Equal("scheduled", body.RootElement.GetProperty("schedule_sms_status").GetString());
        Assert.Equal("Your campaign has been scheduled", response.Message);
        Assert.Equal("C714360330258", response.CampaignId);
        Assert.Equal("success", response.Status);
    }

    [Fact]
    public async Task GetCampaignsAsyncAddsFiltersAndDeserializesResponse()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "content": [
                {
                  "campaign_id": "C714360330258",
                  "run_at": "07-08-2026 13:00",
                  "status": "DELIVERED",
                  "created_at": 1754568051635,
                  "phone_book": "Customers",
                  "camp_type": "regular",
                  "total_recipients": 12
                }
              ],
              "totalPages": 1,
              "totalElements": 1,
              "size": 15,
              "number": 0
            }
            """);
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Campaigns.GetCampaignsAsync(
            new GetCampaignsRequest
            {
                Page = 0,
                Size = 15,
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        var campaign = Assert.Single(response.Content);

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal(
            "https://example.test/api/sms/campaigns?page=0&size=15&api_key=test-api-key",
            request.RequestUri!.AbsoluteUri);
        Assert.Null(request.Content);
        Assert.Equal(1, response.TotalElements);
        Assert.Equal("C714360330258", campaign.CampaignId);
        Assert.Equal("07-08-2026 13:00", campaign.RunAt);
        Assert.Equal("DELIVERED", campaign.Status);
        Assert.Equal(1754568051635, campaign.CreatedAt);
        Assert.Equal("Customers", campaign.Phonebook);
        Assert.Equal("regular", campaign.CampaignType);
        Assert.Equal(12, campaign.TotalRecipients);
    }

    [Fact]
    public async Task GetCampaignHistoryAsyncAddsEscapedCampaignIdAndApiKey()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "id": "688a686cbf5cd77880287d35",
              "applicationId": 33217,
              "campaignId": "C266806497506",
              "phonebookId": "pb-123",
              "phonebookName": "Customers",
              "sender": "Termii",
              "message": "Hello",
              "countryCode": "234",
              "smsType": "plain",
              "campaignType": "regular",
              "status": "DELIVERED",
              "cost": 18.0,
              "totalRecipient": 3,
              "totalDelivered": 2,
              "totalFailed": 1,
              "sent": 3,
              "runAt": "30-07-2026 19:46",
              "isLinkTrackingEnabled": true,
              "rerun": false,
              "sendBy": "sender",
              "personalized": false
            }
            """);
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Campaigns.GetCampaignHistoryAsync("C 123", CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("https://example.test/api/sms/campaigns/C%20123?api_key=test-api-key", request.RequestUri!.AbsoluteUri);
        Assert.Null(request.Content);
        Assert.Equal("688a686cbf5cd77880287d35", response.Id);
        Assert.Equal(33217, response.ApplicationId);
        Assert.Equal("C266806497506", response.CampaignId);
        Assert.Equal("pb-123", response.PhonebookId);
        Assert.Equal("Customers", response.PhonebookName);
        Assert.Equal("DELIVERED", response.Status);
        Assert.Equal(18.0m, response.Cost);
        Assert.Equal(3, response.TotalRecipient);
        Assert.Equal(2, response.TotalDelivered);
        Assert.Equal(1, response.TotalFailed);
        Assert.True(response.IsLinkTrackingEnabled);
        Assert.False(response.Rerun);
    }

    [Fact]
    public async Task RetryAsyncPatchesCampaignWithApiKeyBody()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"message":"Your failed campaign has been retried","status":"success"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Campaigns.RetryAsync("C 123", CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(new HttpMethod("PATCH"), request.Method);
        Assert.Equal("https://example.test/api/sms/campaigns/C%20123", request.RequestUri!.AbsoluteUri);
        var property = Assert.Single(body.RootElement.EnumerateObject());
        Assert.Equal("api_key", property.Name);
        Assert.Equal("test-api-key", property.Value.GetString());
        Assert.Equal("Your failed campaign has been retried", response.Message);
        Assert.Equal("success", response.Status);
    }

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

    [Fact]
    public async Task SendAsyncRejectsMissingRequiredFields()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Campaigns.SendAsync(
            new SendCampaignRequest
            {
                CountryCode = "",
                SenderId = "Termii",
                Message = "Welcome",
                Channel = "generic",
                MessageType = "Plain",
                PhonebookId = "pb-123",
                CampaignType = "regular",
                ScheduleSmsStatus = "regular",
            },
            CancellationToken.None));
    }
}
