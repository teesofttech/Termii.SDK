namespace Termii;

public interface ITermiiCampaignClient
{
    Task<SendCampaignResponse> SendAsync(
        SendCampaignRequest request,
        CancellationToken cancellationToken = default);

    Task<CampaignListResponse> GetCampaignsAsync(
        GetCampaignsRequest? request = null,
        CancellationToken cancellationToken = default);

    Task<CampaignHistoryResponse> GetCampaignHistoryAsync(
        string campaignId,
        CancellationToken cancellationToken = default);

    Task<CampaignOperationResponse> RetryAsync(
        string campaignId,
        CancellationToken cancellationToken = default);

    Task<PhonebookListResponse> GetPhonebooksAsync(
        GetPhonebooksRequest? request = null,
        CancellationToken cancellationToken = default);

    Task<PhonebookResponse> CreatePhonebookAsync(
        CreatePhonebookRequest request,
        CancellationToken cancellationToken = default);

    Task<PhonebookResponse> UpdatePhonebookAsync(
        string phonebookId,
        UpdatePhonebookRequest request,
        CancellationToken cancellationToken = default);

    Task<PhonebookOperationResponse> DeletePhonebookAsync(
        string phonebookId,
        CancellationToken cancellationToken = default);
}
