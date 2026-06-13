namespace Termii;

/// <summary>
/// Configuration required to call the Termii API.
/// </summary>
public sealed class TermiiOptions
{
    public const string DefaultBaseUrl = "https://api.ng.termii.com";

    public string ApiKey { get; set; } = string.Empty;

    public Uri BaseUrl { get; set; } = new(DefaultBaseUrl);

    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);

    internal void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new InvalidOperationException("A Termii API key is required.");
        }

        if (BaseUrl is null || !BaseUrl.IsAbsoluteUri)
        {
            throw new InvalidOperationException("The Termii base URL must be absolute.");
        }

        if (Timeout <= TimeSpan.Zero)
        {
            throw new InvalidOperationException("The Termii timeout must be greater than zero.");
        }
    }
}
