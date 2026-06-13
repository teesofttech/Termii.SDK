namespace Termii;

public interface ITermiiSenderIdClient
{
    Task<SenderIdListResponse> GetAsync(
        GetSenderIdsRequest? request = null,
        CancellationToken cancellationToken = default);

    Task<RequestSenderIdResponse> RequestAsync(
        RequestSenderIdRequest request,
        CancellationToken cancellationToken = default);
}
