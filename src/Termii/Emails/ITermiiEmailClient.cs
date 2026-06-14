namespace Termii;

public interface ITermiiEmailClient
{
    Task<SendProductEmailResponse> SendProductEmailAsync(
        SendProductEmailRequest request,
        CancellationToken cancellationToken = default);
}
