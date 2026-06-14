using System.Net;
using System.Text;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiContactClientTests
{
    [Fact]
    public async Task GetAsyncAddsPhonebookIdFiltersAndApiKey()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "headers": ["phone number"],
              "phonebook": {
                "id": "pb-123",
                "phonebook_name": "Customers",
                "total_contact": 12
              },
              "data": {
                "content": [
                  {
                    "id": "contact-123",
                    "pid": "pb-123",
                    "phone_number": "2348139538813",
                    "contact_list_key_value": [
                      {
                        "key": "phone number",
                        "value": "8139538813"
                      }
                    ]
                  }
                ],
                "totalElements": 1,
                "totalPages": 1,
                "size": 15,
                "number": 0
              }
            }
            """);
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Contacts.GetAsync(
            "pb 123",
            new GetContactsRequest
            {
                Page = 0,
                Size = 15,
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        var contact = Assert.Single(response.Data!.Content);
        var header = Assert.Single(response.Headers);

        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal(
            "https://example.test/api/phonebooks/pb%20123/contacts?page=0&size=15&api_key=test-api-key",
            request.RequestUri!.AbsoluteUri);
        Assert.Null(request.Content);
        Assert.Equal("phone number", header);
        Assert.Equal("Customers", response.Phonebook!.PhonebookName);
        Assert.Equal("contact-123", contact.Id);
        Assert.Equal("2348139538813", contact.PhoneNumber);
        Assert.Equal("8139538813", Assert.Single(contact.ContactListKeyValue).Value);
    }

    [Fact]
    public async Task AddAsyncPostsContactBody()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """
            {
              "Contact added successfully": {
                "id": "contact-123",
                "company": "Termii",
                "phone_number": "2347068410455",
                "email_address": "test@example.com",
                "first_name": "Promise",
                "last_name": "John"
              }
            }
            """);
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Contacts.AddAsync(
            "pb-123",
            new AddContactRequest
            {
                PhoneNumber = "8123696237",
                CountryCode = "234",
                EmailAddress = "test@example.com",
                FirstName = "Promise",
                LastName = "John",
                Company = "Termii",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/phonebooks/pb-123/contacts", request.RequestUri!.AbsoluteUri);
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("8123696237", body.RootElement.GetProperty("phone_number").GetString());
        Assert.Equal("234", body.RootElement.GetProperty("country_code").GetString());
        Assert.Equal("test@example.com", body.RootElement.GetProperty("email_address").GetString());
        Assert.Equal("Promise", body.RootElement.GetProperty("first_name").GetString());
        Assert.Equal("John", body.RootElement.GetProperty("last_name").GetString());
        Assert.Equal("Termii", body.RootElement.GetProperty("company").GetString());
        Assert.Equal("contact-123", response.ContactAddedSuccessfully!.Id);
        Assert.Equal("2347068410455", response.ContactAddedSuccessfully.PhoneNumber);
    }

    [Fact]
    public async Task UploadAsyncPostsMultipartFileAndContactMetadata()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"message":"Your list is being uploaded in the background."}""");
        var client = TestTermiiClientFactory.Create(handler);
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes("phone number\n8123696237"));

        var response = await client.Contacts.UploadAsync(
            new UploadContactsRequest
            {
                PhonebookId = "pb-123",
                CountryCode = "234",
                File = stream,
                FileName = "contacts.csv",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        var body = await request.Content!.ReadAsStringAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/phonebooks/contacts/upload", request.RequestUri!.AbsoluteUri);
        Assert.Equal("multipart/form-data", request.Content.Headers.ContentType!.MediaType);
        Assert.Contains("name=file", body, StringComparison.Ordinal);
        Assert.Contains("filename=contacts.csv", body, StringComparison.Ordinal);
        Assert.Contains("phone number", body, StringComparison.Ordinal);
        Assert.Contains("8123696237", body, StringComparison.Ordinal);
        Assert.Contains("name=contact", body, StringComparison.Ordinal);
        Assert.Contains("application/json", body, StringComparison.Ordinal);
        Assert.Contains("\"pid\":\"pb-123\"", body, StringComparison.Ordinal);
        Assert.Contains("\"country_code\":\"234\"", body, StringComparison.Ordinal);
        Assert.Contains("\"api_key\":\"test-api-key\"", body, StringComparison.Ordinal);
        Assert.Equal("Your list is being uploaded in the background.", response.Message);
    }

    [Fact]
    public async Task DeleteAsyncAddsApiKeyToQueryString()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"code":200,"data":{"message":"Contact has been deleted"},"message":"Deleted Successfully","status":"success"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Contacts.DeleteAsync("pb-123", CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);

        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal("https://example.test/api/phonebooks/pb-123/contacts?api_key=test-api-key", request.RequestUri!.AbsoluteUri);
        Assert.Null(request.Content);
        Assert.Equal(200, response.Code);
        Assert.Equal("Contact has been deleted", response.Data!.Message);
        Assert.Equal("Deleted Successfully", response.Message);
        Assert.Equal("success", response.Status);
    }

    [Fact]
    public async Task AddAsyncRejectsMissingPhoneNumber()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Contacts.AddAsync(
            "pb-123",
            new AddContactRequest
            {
                PhoneNumber = "",
            },
            CancellationToken.None));
    }

    [Fact]
    public async Task UploadAsyncRejectsMissingFile()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Contacts.UploadAsync(
            new UploadContactsRequest
            {
                PhonebookId = "pb-123",
                CountryCode = "234",
                File = Stream.Null,
            },
            CancellationToken.None));
    }
}
