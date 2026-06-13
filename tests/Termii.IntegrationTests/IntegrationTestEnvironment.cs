using Termii;

namespace Termii.IntegrationTests;

internal static class IntegrationTestEnvironment
{
    public static string? ApiKey => Environment.GetEnvironmentVariable("TERMII_API_KEY");

    public static string? BaseUrl => Environment.GetEnvironmentVariable("TERMII_BASE_URL");

    public static string? TestPhoneNumber => Environment.GetEnvironmentVariable("TERMII_TEST_PHONE_NUMBER");

    public static bool HasCredentials => !string.IsNullOrWhiteSpace(ApiKey);

    public static TermiiOptions CreateOptions()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new InvalidOperationException("TERMII_API_KEY is required for live Termii integration tests.");
        }

        var options = new TermiiOptions
        {
            ApiKey = ApiKey,
        };

        if (!string.IsNullOrWhiteSpace(BaseUrl))
        {
            options.BaseUrl = new Uri(BaseUrl);
        }

        return options;
    }
}
