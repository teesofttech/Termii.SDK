namespace Termii;

public interface ITermiiContactClient
{
    Task<ContactListResponse> GetAsync(
        string phonebookId,
        GetContactsRequest? request = null,
        CancellationToken cancellationToken = default);

    Task<ContactResponse> AddAsync(
        string phonebookId,
        AddContactRequest request,
        CancellationToken cancellationToken = default);

    Task<ContactUploadResponse> UploadAsync(
        UploadContactsRequest request,
        CancellationToken cancellationToken = default);

    Task<ContactOperationResponse> DeleteAsync(
        string phonebookId,
        CancellationToken cancellationToken = default);
}
