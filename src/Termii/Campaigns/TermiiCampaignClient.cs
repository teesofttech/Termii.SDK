namespace Termii;

internal sealed class TermiiCampaignClient : ITermiiCampaignClient
{
    private static readonly HttpMethod Patch = new("PATCH");

    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiCampaignClient(TermiiJsonHttpPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public Task<SendCampaignResponse> SendAsync(
        SendCampaignRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<SendCampaignResponse>(
            HttpMethod.Post,
            "/api/sms/campaigns/send",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<CampaignListResponse> GetCampaignsAsync(
        GetCampaignsRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        return _pipeline.SendJsonAsync<CampaignListResponse>(
            HttpMethod.Get,
            (request ?? new GetCampaignsRequest()).ToPath(),
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<CampaignHistoryResponse> GetCampaignHistoryAsync(
        string campaignId,
        CancellationToken cancellationToken = default)
    {
        TermiiRequestValidation.Required(campaignId, nameof(campaignId));

        return _pipeline.SendJsonAsync<CampaignHistoryResponse>(
            HttpMethod.Get,
            $"/api/sms/campaigns/{Uri.EscapeDataString(campaignId)}",
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<CampaignOperationResponse> RetryAsync(
        string campaignId,
        CancellationToken cancellationToken = default)
    {
        TermiiRequestValidation.Required(campaignId, nameof(campaignId));

        return _pipeline.SendJsonAsync<CampaignOperationResponse>(
            Patch,
            $"/api/sms/campaigns/{Uri.EscapeDataString(campaignId)}",
            body: null,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<PhonebookListResponse> GetPhonebooksAsync(
        GetPhonebooksRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        return _pipeline.SendJsonAsync<PhonebookListResponse>(
            HttpMethod.Get,
            (request ?? new GetPhonebooksRequest()).ToPath(),
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<PhonebookResponse> CreatePhonebookAsync(
        CreatePhonebookRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<PhonebookResponse>(
            HttpMethod.Post,
            "/api/phonebooks",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<PhonebookResponse> UpdatePhonebookAsync(
        string phonebookId,
        UpdatePhonebookRequest request,
        CancellationToken cancellationToken = default)
    {
        TermiiRequestValidation.Required(phonebookId, nameof(phonebookId));

        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return _pipeline.SendJsonAsync<PhonebookResponse>(
            Patch,
            $"/api/phonebooks/{Uri.EscapeDataString(phonebookId)}",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<PhonebookOperationResponse> DeletePhonebookAsync(
        string phonebookId,
        CancellationToken cancellationToken = default)
    {
        TermiiRequestValidation.Required(phonebookId, nameof(phonebookId));

        return _pipeline.SendJsonAsync<PhonebookOperationResponse>(
            HttpMethod.Delete,
            $"/api/phonebooks/{Uri.EscapeDataString(phonebookId)}",
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }
}
