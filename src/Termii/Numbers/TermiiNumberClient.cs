namespace Termii;

internal sealed class TermiiNumberClient : ITermiiNumberClient
{
    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiNumberClient(TermiiJsonHttpPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public Task<TermiiMessageResponse> SendAsync(
        SendNumberMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<TermiiMessageResponse>(
            HttpMethod.Post,
            "/api/sms/number/send",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }
}
