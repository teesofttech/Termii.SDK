using System.Text.Json.Serialization;

namespace Termii;

internal sealed class TermiiTokenClient : ITermiiTokenClient
{
    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiTokenClient(TermiiJsonHttpPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    public Task<SendTokenResponse> SendAsync(
        SendTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<SendTokenResponse>(
            HttpMethod.Post,
            "/api/sms/otp/send",
            new SendTokenPayload(request),
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<VerifyTokenResponse> VerifyAsync(
        VerifyTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<VerifyTokenResponse>(
            HttpMethod.Post,
            "/api/sms/otp/verify",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<GenerateTokenResponse> GenerateAsync(
        GenerateTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<GenerateTokenResponse>(
            HttpMethod.Post,
            "/api/sms/otp/generate",
            new GenerateTokenPayload(request),
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<TermiiMessageResponse> SendVoiceAsync(
        SendVoiceTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<TermiiMessageResponse>(
            HttpMethod.Post,
            "/api/sms/otp/send/voice",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<TermiiMessageResponse> CallAsync(
        VoiceCallTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<TermiiMessageResponse>(
            HttpMethod.Post,
            "/api/sms/otp/call",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<TermiiMessageResponse> SendEmailAsync(
        SendEmailTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.Validate();

        return _pipeline.SendJsonAsync<TermiiMessageResponse>(
            HttpMethod.Post,
            "/api/email/otp/send",
            request,
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    public Task<TermiiMessageResponse> SendWhatsAppAsync(
        SendWhatsAppTokenRequest request,
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
            new SendWhatsAppTokenPayload(request),
            TermiiAuthenticationLocation.Body,
            cancellationToken);
    }

    private sealed class SendTokenPayload
    {
        public SendTokenPayload(SendTokenRequest request)
        {
            MessageType = request.PinType.ToWireValue();
            PinType = request.PinType.ToWireValue();
            To = request.To;
            From = request.From;
            Channel = request.Channel.ToWireValue();
            PinAttempts = request.PinAttempts;
            PinTimeToLive = request.PinTimeToLive;
            PinLength = request.PinLength;
            PinPlaceholder = request.PinPlaceholder;
            MessageText = request.MessageText;
        }

        [JsonPropertyName("message_type")]
        public string MessageType { get; }

        [JsonPropertyName("pin_type")]
        public string PinType { get; }

        [JsonPropertyName("to")]
        public string To { get; }

        [JsonPropertyName("from")]
        public string From { get; }

        [JsonPropertyName("channel")]
        public string Channel { get; }

        [JsonPropertyName("pin_attempts")]
        public int PinAttempts { get; }

        [JsonPropertyName("pin_time_to_live")]
        public int PinTimeToLive { get; }

        [JsonPropertyName("pin_length")]
        public int PinLength { get; }

        [JsonPropertyName("pin_placeholder")]
        public string PinPlaceholder { get; }

        [JsonPropertyName("message_text")]
        public string MessageText { get; }
    }

    private sealed class GenerateTokenPayload
    {
        public GenerateTokenPayload(GenerateTokenRequest request)
        {
            PinType = request.PinType.ToWireValue();
            PhoneNumber = request.PhoneNumber;
            PinAttempts = request.PinAttempts;
            PinTimeToLive = request.PinTimeToLive;
            PinLength = request.PinLength;
        }

        [JsonPropertyName("pin_type")]
        public string PinType { get; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; }

        [JsonPropertyName("pin_attempts")]
        public int PinAttempts { get; }

        [JsonPropertyName("pin_time_to_live")]
        public int PinTimeToLive { get; }

        [JsonPropertyName("pin_length")]
        public int PinLength { get; }
    }

    private sealed class SendWhatsAppTokenPayload
    {
        public SendWhatsAppTokenPayload(SendWhatsAppTokenRequest request)
        {
            To = request.To;
            From = request.From;
            Sms = request.Sms;
            Type = request.Type.ToWireValue();
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
        public string Channel => "whatsapp_otp";
    }
}
