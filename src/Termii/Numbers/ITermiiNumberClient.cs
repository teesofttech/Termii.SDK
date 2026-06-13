namespace Termii;

public interface ITermiiNumberClient
{
    Task<TermiiMessageResponse> SendAsync(
        SendNumberMessageRequest request,
        CancellationToken cancellationToken = default);
}
