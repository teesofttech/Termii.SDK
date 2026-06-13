# Termii .NET SDK

[![CI](https://github.com/teesofttech/Termii.SDK/actions/workflows/ci.yml/badge.svg)](https://github.com/teesofttech/Termii.SDK/actions/workflows/ci.yml)

A .NET SDK for the Termii messaging, sender ID, Number API, Token API, and Insights APIs.

## Installation

Install from NuGet:

```bash
dotnet add package Termii.SDK
```

Then import the SDK namespace:

```csharp
using Termii;
```

## Configuration

Create a client manually:

```csharp
var client = new TermiiClient(new TermiiOptions
{
    ApiKey = "your-termii-api-key"
});
```

The default base URL is `https://api.ng.termii.com`. Override it only when your Termii account or environment requires a different base URL:

```csharp
var client = new TermiiClient(new TermiiOptions
{
    ApiKey = "your-termii-api-key",
    BaseUrl = new Uri("https://api.ng.termii.com"),
    Timeout = TimeSpan.FromSeconds(30)
});
```

For ASP.NET Core applications, register `TermiiClient` with dependency injection:

```csharp
builder.Services.AddTermii(options =>
{
    options.ApiKey = builder.Configuration["Termii:ApiKey"]!;
});
```

Use it from a service or endpoint:

```csharp
public sealed class NotificationService
{
    private readonly TermiiClient _termii;

    public NotificationService(TermiiClient termii)
    {
        _termii = termii;
    }

    public Task<TermiiMessageResponse> SendAsync(string phoneNumber, string message)
    {
        return _termii.Messaging.SendAsync(new SendMessageRequest
        {
            To = phoneNumber,
            From = "Termii",
            Sms = message,
            Channel = TermiiMessageChannel.Generic,
            Type = TermiiMessageType.Plain
        });
    }
}
```

## Messaging

Send a single SMS:

```csharp
var response = await client.Messaging.SendAsync(new SendMessageRequest
{
    To = "2348012345678",
    From = "Termii",
    Sms = "Hello from .NET",
    Channel = TermiiMessageChannel.Generic,
    Type = TermiiMessageType.Plain
});
```

Send a bulk SMS:

```csharp
var response = await client.Messaging.SendBulkAsync(new SendBulkMessageRequest
{
    To = new[] { "2348012345678", "2348098765432" },
    From = "Termii",
    Sms = "Hello everyone",
    Channel = TermiiMessageChannel.Generic,
    Type = TermiiMessageType.Plain
});
```

Send through the Number API:

```csharp
var response = await client.Numbers.SendAsync(new SendNumberMessageRequest
{
    To = "2348012345678",
    Sms = "Hello from a dedicated Termii number"
});
```

## Sender IDs

Fetch sender IDs:

```csharp
var senderIds = await client.SenderIds.GetAsync();
```

Request a sender ID:

```csharp
var request = await client.SenderIds.RequestAsync(new RequestSenderIdRequest
{
    SenderId = "Termii",
    UseCase = "Transactional alerts",
    Company = "Example Ltd"
});
```

## Tokens and OTP

Send an OTP token:

```csharp
var token = await client.Tokens.SendAsync(new SendTokenRequest
{
    To = "2348012345678",
    From = "Termii",
    Channel = TermiiMessageChannel.Generic,
    PinAttempts = 3,
    PinTimeToLive = 10,
    PinLength = 6,
    PinPlaceholder = "< 123456 >",
    MessageText = "Your verification code is < 123456 >"
});
```

Verify an OTP token:

```csharp
var verification = await client.Tokens.VerifyAsync(new VerifyTokenRequest
{
    PinId = token.PinId!,
    Pin = "123456"
});
```

Generate an in-app OTP:

```csharp
var generated = await client.Tokens.GenerateAsync(new GenerateTokenRequest
{
    PhoneNumber = "2348012345678",
    PinType = TermiiTokenPinType.Numeric,
    PinAttempts = 3,
    PinTimeToLive = 10,
    PinLength = 6
});
```

The token client also supports voice OTP, voice call OTP, email OTP, and WhatsApp OTP through `SendVoiceAsync`, `CallAsync`, `SendEmailAsync`, and `SendWhatsAppAsync`.

## Insights

Check account balance:

```csharp
var balance = await client.Insights.GetBalanceAsync();
```

Check DND status:

```csharp
var dnd = await client.Insights.CheckDndAsync(new CheckDndRequest
{
    PhoneNumber = "2348012345678"
});
```

Query number intelligence:

```csharp
var number = await client.Insights.QueryNumberAsync(new QueryNumberRequest
{
    PhoneNumber = "2348012345678",
    CountryCode = "NG"
});
```

Fetch message history:

```csharp
var history = await client.Insights.GetMessageHistoryAsync(new GetMessageHistoryRequest
{
    Page = 0,
    Size = 15,
    Sender = "Termii",
    Receiver = "2348012345678"
});
```

## Error Handling

The SDK throws `TermiiApiException` for non-success HTTP responses from Termii:

```csharp
try
{
    await client.Messaging.SendAsync(new SendMessageRequest
    {
        To = "2348012345678",
        From = "Termii",
        Sms = "Hello from .NET",
        Channel = TermiiMessageChannel.Generic
    });
}
catch (TermiiApiException ex)
{
    Console.WriteLine($"HTTP status: {(int)ex.StatusCode}");
    Console.WriteLine($"Termii code: {ex.TermiiCode}");
    Console.WriteLine($"Termii message: {ex.TermiiMessage}");
}
```

Request models also validate required fields before sending. Missing required values throw `ArgumentException`, and invalid numeric ranges throw `ArgumentOutOfRangeException`.

## Supported APIs

Implemented in the current SDK:

- Messaging: send single, WhatsApp conversational, and bulk messages.
- Sender IDs: list and request sender IDs.
- Number API: send message through a dedicated Termii number.
- Tokens: send, verify, generate, voice, email, and WhatsApp OTP flows.
- Insights: balance, DND status, number intelligence, and message history.

Deferred or not yet implemented:

- WhatsApp template/device message APIs.
- Campaign phonebook APIs.
- Product notification email APIs.
- Webhook event models.
- `/api/sms/history/analytics`, pending additional verification.

See [docs/API_COVERAGE.md](docs/API_COVERAGE.md) for the detailed coverage matrix.

## Examples

Run the examples project after setting your Termii API key:

```bash
export TERMII_API_KEY="your-termii-api-key"
dotnet run --project examples/Termii.Examples
```

Run a live balance example:

```bash
export TERMII_EXAMPLE_ACTION="balance"
dotnet run --project examples/Termii.Examples
```

Send a live Number API example message:

```bash
export TERMII_EXAMPLE_ACTION="number-message"
export TERMII_EXAMPLE_PHONE_NUMBER="2348012345678"
dotnet run --project examples/Termii.Examples
```

## Tests

Run the local test suite:

```bash
dotnet test
```

Integration tests are opt-in and must not call live Termii endpoints unless the required environment variables are present:

```bash
export TERMII_API_KEY="your-termii-api-key"
export TERMII_BASE_URL="https://api.ng.termii.com"
export TERMII_TEST_PHONE_NUMBER="2348012345678"
dotnet test tests/Termii.IntegrationTests
```

## Release

Releases are published from version tags through GitHub Actions. See [docs/RELEASING.md](docs/RELEASING.md) for the checklist.

## Links

- Termii developer docs: https://developer.termii.com/
- NuGet package: https://www.nuget.org/packages/Termii.SDK
- API coverage: [docs/API_COVERAGE.md](docs/API_COVERAGE.md)

## License

This project is licensed under the MIT License.
