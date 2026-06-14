using System.Text.Json.Serialization;

namespace Termii;

internal sealed class TermiiMessagingClient : ITermiiMessagingClient
{
    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiMessagingClient(TermiiJsonHttpPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public Task<TermiiMessageResponse> SendAsync(
        SendMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<TermiiMessageResponse>(
            HttpMethod.Post,
            "/api/sms/send",
            new SendMessagePayload(request),
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<TermiiMessageResponse> SendBulkAsync(
        SendBulkMessageRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<TermiiMessageResponse>(
            HttpMethod.Post,
            "/api/sms/send/bulk",
            new SendBulkMessagePayload(request),
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<WhatsAppTemplateResponse> SendWhatsAppTemplateAsync(
        SendWhatsAppTemplateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<WhatsAppTemplateResponse>(
            HttpMethod.Post,
            "/api/send/template",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<WhatsAppTemplateResponse> SendWhatsAppTemplateMediaAsync(
        SendWhatsAppTemplateMediaRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<WhatsAppTemplateResponse>(
            HttpMethod.Post,
            "/api/send/template/media",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    private sealed class SendMessagePayload
    {
        public SendMessagePayload(SendMessageRequest request)
        {
            To = request.To;
            From = request.From;
            Sms = request.Sms;
            Type = request.Type.ToWireValue();
            Channel = request.Channel.ToWireValue();
            Media = request.Media;
        }

        [JsonPropertyName("to")]
        public string To { get; }

        [JsonPropertyName("from")]
        public string From { get; }

        [JsonPropertyName("sms")]
        public string Sms { get; }

        [JsonPropertyName("type")]
        public string Type { get; }

        [JsonPropertyName("channel")]
        public string Channel { get; }

        [JsonPropertyName("media")]
        public TermiiMessageMedia? Media { get; }
    }

    private sealed class SendBulkMessagePayload
    {
        public SendBulkMessagePayload(SendBulkMessageRequest request)
        {
            To = request.To;
            From = request.From;
            Sms = request.Sms;
            Type = request.Type.ToWireValue();
            Channel = request.Channel.ToWireValue();
        }

        [JsonPropertyName("to")]
        public IReadOnlyCollection<string> To { get; }

        [JsonPropertyName("from")]
        public string From { get; }

        [JsonPropertyName("sms")]
        public string Sms { get; }

        [JsonPropertyName("type")]
        public string Type { get; }

        [JsonPropertyName("channel")]
        public string Channel { get; }
    }
}
