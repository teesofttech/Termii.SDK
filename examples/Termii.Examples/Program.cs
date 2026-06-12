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
