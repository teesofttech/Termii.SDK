# Termii .NET SDK

[![CI](https://github.com/teesofttech/Termii.SDK/actions/workflows/ci.yml/badge.svg)](https://github.com/teesofttech/Termii.SDK/actions/workflows/ci.yml)

A .NET SDK for the Termii messaging, token, and insights APIs.

> This SDK is in early development. The first milestones establish the client, tests, examples, API coverage matrix, and package structure before endpoint support is added feature by feature.

## Current Status

The initial repository setup includes:

- A `Termii` SDK project targeting `netstandard2.0` and `net8.0`.
- A lightweight unit test project.
- An examples project for developer-facing usage.
- An API coverage matrix in `docs/API_COVERAGE.md`.

## Usage

Install the package:

```bash
dotnet add package Termii.SDK
```

```csharp
using Termii;

var client = new TermiiClient(new TermiiOptions
{
    ApiKey = "your-termii-api-key"
});
```

The default base URL is `https://api.ng.termii.com`.

For ASP.NET Core applications, register the SDK with dependency injection:

```csharp
builder.Services.AddTermii(options =>
{
    options.ApiKey = builder.Configuration["Termii:ApiKey"]!;
});
```

Send a message:

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

## Examples

Run the examples project after setting your Termii API key:

```bash
export TERMII_API_KEY="your-termii-api-key"
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

As endpoint support is added, integration tests should remain credential-gated so normal CI can run without network access or Termii credits.

## Roadmap

Development is tracked through GitHub issues:

- SDK foundation and HTTP pipeline.
- Messaging APIs.
- Sender ID and number APIs.
- Token/OTP APIs.
- Insights APIs.
- CI, documentation, NuGet packaging, and GitHub Releases.

Official Termii documentation: https://developer.termii.com/

## License

This project is licensed under the MIT License.
