namespace Termii;

public interface ITermiiCampaignClient
{
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
