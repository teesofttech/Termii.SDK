using System.Net;
using Termii;
using Termii.Tests.Infrastructure;
using Xunit;

namespace Termii.Tests;

public sealed class TermiiTokenClientTests
{
    [Fact]
    public async Task SendAsyncPostsTokenBodyAndDeserializesResponse()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"smsStatus":"Message Sent","to":"2348012345678","pinId":"pin-123","message_id_str":"msg-123","status":"success"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Tokens.SendAsync(
            new SendTokenRequest
            {
                To = "2348012345678",
                From = "Termii",
                Channel = TermiiMessageChannel.Dnd,
                PinAttempts = 3,
                PinTimeToLive = 10,
                PinLength = 6,
                PinPlaceholder = "< 1234 >",
                MessageText = "Your pin is < 1234 >",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://example.test/api/sms/otp/send", request.RequestUri!.ToString());
        Assert.Equal("test-api-key", body.RootElement.GetProperty("api_key").GetString());
        Assert.Equal("NUMERIC", body.RootElement.GetProperty("message_type").GetString());
        Assert.Equal("NUMERIC", body.RootElement.GetProperty("pin_type").GetString());
        Assert.Equal("2348012345678", body.RootElement.GetProperty("to").GetString());
        Assert.Equal("Termii", body.RootElement.GetProperty("from").GetString());
        Assert.Equal("dnd", body.RootElement.GetProperty("channel").GetString());
        Assert.Equal(3, body.RootElement.GetProperty("pin_attempts").GetInt32());
        Assert.Equal(10, body.RootElement.GetProperty("pin_time_to_live").GetInt32());
        Assert.Equal(6, body.RootElement.GetProperty("pin_length").GetInt32());
        Assert.Equal("< 1234 >", body.RootElement.GetProperty("pin_placeholder").GetString());
        Assert.Equal("Your pin is < 1234 >", body.RootElement.GetProperty("message_text").GetString());

        Assert.Equal("Message Sent", response.SmsStatus);
        Assert.Equal("pin-123", response.PinId);
        Assert.Equal("msg-123", response.MessageIdString);
        Assert.Equal("success", response.Status);
    }

    [Fact]
    public async Task VerifyAsyncPostsPinDetails()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"pinId":"pin-123","verified":"true","msisdn":"2348012345678"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Tokens.VerifyAsync(
            new VerifyTokenRequest
            {
                PinId = "pin-123",
                Pin = "123456",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal("https://example.test/api/sms/otp/verify", request.RequestUri!.ToString());
        Assert.Equal("pin-123", body.RootElement.GetProperty("pin_id").GetString());
        Assert.Equal("123456", body.RootElement.GetProperty("pin").GetString());
        Assert.Equal("true", response.Verified);
        Assert.Equal("2348012345678", response.Msisdn);
    }

    [Fact]
    public async Task GenerateAsyncPostsInAppTokenBody()
    {
        using var handler = new TestHttpMessageHandler(
            HttpStatusCode.OK,
            """{"phone_number":"2348012345678","otp":"123456","pin_id":"pin-123"}""");
        var client = TestTermiiClientFactory.Create(handler);

        var response = await client.Tokens.GenerateAsync(
            new GenerateTokenRequest
            {
                PinType = TermiiTokenPinType.Alphanumeric,
                PhoneNumber = "2348012345678",
                PinAttempts = 3,
                PinTimeToLive = 10,
                PinLength = 6,
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal("https://example.test/api/sms/otp/generate", request.RequestUri!.ToString());
        Assert.Equal("ALPHANUMERIC", body.RootElement.GetProperty("pin_type").GetString());
        Assert.Equal("2348012345678", body.RootElement.GetProperty("phone_number").GetString());
        Assert.Equal("123456", response.Otp);
        Assert.Equal("pin-123", response.PinId);
    }

    [Fact]
    public async Task SendVoiceAsyncPostsVoiceTokenBody()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await client.Tokens.SendVoiceAsync(
            new SendVoiceTokenRequest
            {
                PhoneNumber = "2348012345678",
                PinAttempts = 2,
                PinTimeToLive = 5,
                PinLength = 4,
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal("https://example.test/api/sms/otp/send/voice", request.RequestUri!.ToString());
        Assert.Equal("2348012345678", body.RootElement.GetProperty("phone_number").GetString());
        Assert.Equal(2, body.RootElement.GetProperty("pin_attempts").GetInt32());
    }

    [Fact]
    public async Task CallAsyncPostsVoiceCallTokenBody()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await client.Tokens.CallAsync(
            new VoiceCallTokenRequest
            {
                PhoneNumber = "2348012345678",
                Code = "123456",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal("https://example.test/api/sms/otp/call", request.RequestUri!.ToString());
        Assert.Equal("2348012345678", body.RootElement.GetProperty("phone_number").GetString());
        Assert.Equal("123456", body.RootElement.GetProperty("code").GetString());
    }

    [Fact]
    public async Task SendEmailAsyncPostsEmailTokenBody()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await client.Tokens.SendEmailAsync(
            new SendEmailTokenRequest
            {
                EmailAddress = "person@example.com",
                Code = "123456",
                EmailConfigurationId = "email-config",
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal("https://example.test/api/email/otp/send", request.RequestUri!.ToString());
        Assert.Equal("person@example.com", body.RootElement.GetProperty("email_address").GetString());
        Assert.Equal("123456", body.RootElement.GetProperty("code").GetString());
        Assert.Equal("email-config", body.RootElement.GetProperty("email_configuration_id").GetString());
    }

    [Fact]
    public async Task SendWhatsAppAsyncPostsWhatsAppOtpMessage()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await client.Tokens.SendWhatsAppAsync(
            new SendWhatsAppTokenRequest
            {
                To = "2348012345678",
                From = "Termii",
                Sms = "Your code is 123456",
                Type = TermiiMessageType.Unicode,
            },
            CancellationToken.None);

        var request = handler.LastRequest;
        Assert.NotNull(request);
        using var body = await request.ReadJsonBodyAsync(CancellationToken.None);

        Assert.Equal("https://example.test/api/sms/send", request.RequestUri!.ToString());
        Assert.Equal("whatsapp_otp", body.RootElement.GetProperty("channel").GetString());
        Assert.Equal("unicode", body.RootElement.GetProperty("type").GetString());
        Assert.Equal("Your code is 123456", body.RootElement.GetProperty("sms").GetString());
    }

    [Fact]
    public async Task SendAsyncRejectsInvalidPinLength()
    {
        using var handler = new TestHttpMessageHandler();
        var client = TestTermiiClientFactory.Create(handler);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Tokens.SendAsync(
            new SendTokenRequest
            {
                To = "2348012345678",
                From = "Termii",
                PinAttempts = 3,
                PinTimeToLive = 10,
                PinLength = 3,
                PinPlaceholder = "< 1234 >",
                MessageText = "Your pin is < 1234 >",
            },
            CancellationToken.None));
    }
}
