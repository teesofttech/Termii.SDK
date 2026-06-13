namespace Termii;

internal sealed class TermiiInsightsClient : ITermiiInsightsClient
{
    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiInsightsClient(TermiiJsonHttpPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public Task<GetBalanceResponse> GetBalanceAsync(CancellationToken cancellationToken = default)
    {
        return _pipeline.SendJsonAsync<GetBalanceResponse>(
            HttpMethod.Get,
            "/api/get-balance",
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<CheckDndResponse> CheckDndAsync(
        CheckDndRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return _pipeline.SendJsonAsync<CheckDndResponse>(
            HttpMethod.Get,
            request.ToPath(),
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<QueryNumberResponse> QueryNumberAsync(
        QueryNumberRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return _pipeline.SendJsonAsync<QueryNumberResponse>(
            HttpMethod.Get,
            request.ToPath(),
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<MessageHistoryResponse> GetMessageHistoryAsync(
        GetMessageHistoryRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        return _pipeline.SendJsonAsync<MessageHistoryResponse>(
            HttpMethod.Get,
            (request ?? new GetMessageHistoryRequest()).ToPath(),
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<MessageAnalyticsResponse> GetMessageAnalyticsAsync(
        GetMessageAnalyticsRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        return _pipeline.SendJsonAsync<MessageAnalyticsResponse>(
            HttpMethod.Get,
            (request ?? new GetMessageAnalyticsRequest()).ToPath(),
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }
}
