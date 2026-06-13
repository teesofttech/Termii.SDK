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

var action = Environment.GetEnvironmentVariable("TERMII_EXAMPLE_ACTION");
var phoneNumber = Environment.GetEnvironmentVariable("TERMII_EXAMPLE_PHONE_NUMBER");

if (string.Equals(action, "balance", StringComparison.OrdinalIgnoreCase))
{
    var balance = await client.Insights.GetBalanceAsync();

    Console.WriteLine($"Balance: {balance.Balance} {balance.Currency}".Trim());
    return;
}

if (string.Equals(action, "number-message", StringComparison.OrdinalIgnoreCase))
{
    if (string.IsNullOrWhiteSpace(phoneNumber))
    {
        Console.WriteLine("Set TERMII_EXAMPLE_PHONE_NUMBER to send a Number API message.");
        return;
    }

    var response = await client.Numbers.SendAsync(new SendNumberMessageRequest
    {
        To = phoneNumber,
        Sms = "Hello from Termii.SDK",
    });

    Console.WriteLine($"Number API response: {response.Message ?? response.Code ?? "sent"}");
    return;
}

Console.WriteLine("Set TERMII_EXAMPLE_ACTION=balance or TERMII_EXAMPLE_ACTION=number-message to run a live example.");
