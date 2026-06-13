namespace Termii;

public interface ITermiiTokenClient
{
    Task<SendTokenResponse> SendAsync(
        SendTokenRequest request,
        CancellationToken cancellationToken = default);

    Task<VerifyTokenResponse> VerifyAsync(
        VerifyTokenRequest request,
        CancellationToken cancellationToken = default);

    Task<GenerateTokenResponse> GenerateAsync(
        GenerateTokenRequest request,
        CancellationToken cancellationToken = default);

    Task<TermiiMessageResponse> SendVoiceAsync(
        SendVoiceTokenRequest request,
        CancellationToken cancellationToken = default);

    Task<TermiiMessageResponse> CallAsync(
        VoiceCallTokenRequest request,
        CancellationToken cancellationToken = default);

    Task<TermiiMessageResponse> SendEmailAsync(
        SendEmailTokenRequest request,
        CancellationToken cancellationToken = default);

    Task<TermiiMessageResponse> SendWhatsAppAsync(
        SendWhatsAppTokenRequest request,
        CancellationToken cancellationToken = default);
}
