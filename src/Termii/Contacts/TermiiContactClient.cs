using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Termii;

internal sealed class TermiiContactClient : ITermiiContactClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiContactClient(TermiiJsonHttpPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public Task<ContactListResponse> GetAsync(
        string phonebookId,
        GetContactsRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        TermiiRequestValidation.Required(phonebookId, nameof(phonebookId));

        return _pipeline.SendJsonAsync<ContactListResponse>(
            HttpMethod.Get,
            (request ?? new GetContactsRequest()).ToPath(phonebookId),
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<ContactResponse> AddAsync(
        string phonebookId,
        AddContactRequest request,
        CancellationToken cancellationToken = default)
    {
        TermiiRequestValidation.Required(phonebookId, nameof(phonebookId));

        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<ContactResponse>(
            HttpMethod.Post,
            $"/api/phonebooks/{Uri.EscapeDataString(phonebookId)}/contacts",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<ContactUploadResponse> UploadAsync(
        UploadContactsRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(request.File);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);
        content.Add(fileContent, "file", request.FileName);

        var contact = new
        {
            pid = request.PhonebookId,
            country_code = request.CountryCode,
            api_key = _pipeline.ApiKey,
        };
        content.Add(
            new StringContent(JsonSerializer.Serialize(contact, JsonSerializerOptions), Encoding.UTF8, "application/json"),
            "contact");

        return _pipeline.SendContentJsonAsync<ContactUploadResponse>(
            HttpMethod.Post,
            "/api/phonebooks/contacts/upload",
            content,
            cancellationToken);
    }

    public Task<ContactOperationResponse> DeleteAsync(
        string phonebookId,
        CancellationToken cancellationToken = default)
    {
        TermiiRequestValidation.Required(phonebookId, nameof(phonebookId));

        return _pipeline.SendJsonAsync<ContactOperationResponse>(
            HttpMethod.Delete,
            $"/api/phonebooks/{Uri.EscapeDataString(phonebookId)}/contacts",
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }
}
