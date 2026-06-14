namespace Termii;

public interface ITermiiMessagingClient
{
    Task<TermiiMessageResponse> SendAsync(
        SendMessageRequest request,
        CancellationToken cancellationToken = default);

    Task<TermiiMessageResponse> SendBulkAsync(
        SendBulkMessageRequest request,
        CancellationToken cancellationToken = default);

    Task<WhatsAppTemplateResponse> SendWhatsAppTemplateAsync(
        SendWhatsAppTemplateRequest request,
        CancellationToken cancellationToken = default);

    Task<WhatsAppTemplateResponse> SendWhatsAppTemplateMediaAsync(
        SendWhatsAppTemplateMediaRequest request,
        CancellationToken cancellationToken = default);
}
