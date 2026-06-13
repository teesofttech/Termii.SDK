namespace Termii;

internal sealed class TermiiSenderIdClient : ITermiiSenderIdClient
{
    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiSenderIdClient(TermiiJsonHttpPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public Task<SenderIdListResponse> GetAsync(
        GetSenderIdsRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        return _pipeline.SendJsonAsync<SenderIdListResponse>(
            HttpMethod.Get,
            (request ?? new GetSenderIdsRequest()).ToPath(),
            body: null,
            TermiiAuthenticationLocation.Query,
            cancellationToken);
    }

    public Task<RequestSenderIdResponse> RequestAsync(
        RequestSenderIdRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<RequestSenderIdResponse>(
            HttpMethod.Post,
            "/api/sender-id/request",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }
}
