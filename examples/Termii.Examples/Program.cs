using Termii;

var apiKey = Environment.GetEnvironmentVariable("TERMII_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("Set TERMII_API_KEY to run the Termii examples.");
    return;
}

var client = new TermiiClient(new TermiiOptions
{
    ApiKey = apiKey,
});

Console.WriteLine($"Termii client configured for {client.Options.BaseUrl}.");

var phoneNumber = Environment.GetEnvironmentVariable("TERMII_EXAMPLE_PHONE_NUMBER");
var sendNumberMessage = string.Equals(
    Environment.GetEnvironmentVariable("TERMII_SEND_NUMBER_MESSAGE"),
    "true",
    StringComparison.OrdinalIgnoreCase);

if (!sendNumberMessage || string.IsNullOrWhiteSpace(phoneNumber))
{
    Console.WriteLine("Set TERMII_SEND_NUMBER_MESSAGE=true and TERMII_EXAMPLE_PHONE_NUMBER to send a Number API message.");
    return;
}

var response = await client.Numbers.SendAsync(new SendNumberMessageRequest
{
    To = phoneNumber,
    Sms = "Hello from Termii.SDK",
});

Console.WriteLine($"Number API response: {response.Message ?? response.Code ?? "sent"}");
