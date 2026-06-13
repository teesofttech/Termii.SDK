namespace Termii;

public interface ITermiiInsightsClient
{
    Task<GetBalanceResponse> GetBalanceAsync(CancellationToken cancellationToken = default);

    Task<CheckDndResponse> CheckDndAsync(
        CheckDndRequest request,
        CancellationToken cancellationToken = default);

    Task<QueryNumberResponse> QueryNumberAsync(
        QueryNumberRequest request,
        CancellationToken cancellationToken = default);

    Task<MessageHistoryResponse> GetMessageHistoryAsync(
        GetMessageHistoryRequest? request = null,
        CancellationToken cancellationToken = default);
}
