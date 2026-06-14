using System.Security.Cryptography;
using System.Text;
using Termii;

var action = Environment.GetEnvironmentVariable("TERMII_EXAMPLE_ACTION");

if (string.Equals(action, "webhook-signature", StringComparison.OrdinalIgnoreCase))
{
    const string payload = """{"type":"delivery_report","message_id":"msg-123","status":"delivered"}""";
    const string secretKey = "example-webhook-secret";
    var signature = CreateWebhookSignature(payload, secretKey);
    var verified = TermiiWebhookSignature.Verify(payload, signature, secretKey);

    Console.WriteLine($"Webhook signature verified: {verified}");
    return;
}

var apiKey = Environment.GetEnvironmentVariable("TERMII_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("Set TERMII_API_KEY to run live Termii examples.");
    Console.WriteLine("Set TERMII_EXAMPLE_ACTION=webhook-signature to run the offline webhook signature example.");
    return;
}

var client = new TermiiClient(new TermiiOptions
{
    ApiKey = apiKey,
});

Console.WriteLine($"Termii client configured for {client.Options.BaseUrl}.");

var phoneNumber = Environment.GetEnvironmentVariable("TERMII_EXAMPLE_PHONE_NUMBER");

if (string.Equals(action, "balance", StringComparison.OrdinalIgnoreCase))
{
    var balance = await client.Insights.GetBalanceAsync();

    Console.WriteLine($"Balance: {balance.Balance} {balance.Currency}".Trim());
    return;
}

if (string.Equals(action, "phonebooks-list", StringComparison.OrdinalIgnoreCase))
{
    var phonebooks = await client.Campaigns.GetPhonebooksAsync(new GetPhonebooksRequest
    {
        Page = 0,
        Size = 10,
    });

    Console.WriteLine($"Phonebooks returned: {phonebooks.Content.Count}");
    return;
}

if (string.Equals(action, "campaigns-list", StringComparison.OrdinalIgnoreCase))
{
    var campaigns = await client.Campaigns.GetCampaignsAsync(new GetCampaignsRequest
    {
        Page = 0,
        Size = 10,
    });

    Console.WriteLine($"Campaigns returned: {campaigns.Content.Count}");
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

Console.WriteLine("Set TERMII_EXAMPLE_ACTION to one of: balance, phonebooks-list, campaigns-list, number-message, webhook-signature.");

static string CreateWebhookSignature(string payload, string secretKey)
{
    using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey));
    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
    var builder = new StringBuilder(hash.Length * 2);

    foreach (var value in hash)
    {
        builder.Append(value.ToString("x2"));
    }

    return builder.ToString();
}
