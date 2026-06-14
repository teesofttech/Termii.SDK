namespace Termii;

internal sealed class TermiiEmailClient : ITermiiEmailClient
{
    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiEmailClient(TermiiJsonHttpPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public Task<SendProductEmailResponse> SendProductEmailAsync(
        SendProductEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<SendProductEmailResponse>(
            HttpMethod.Post,
            "/api/templates/send-email",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }
}
