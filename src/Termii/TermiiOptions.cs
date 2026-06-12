namespace Termii;

/// <summary>
/// Configuration required to call the Termii API.
/// </summary>
public sealed class TermiiOptions
{
    public const string DefaultBaseUrl = "https://api.ng.termii.com";

    public string ApiKey { get; set; } = string.Empty;

    public Uri BaseUrl { get; set; } = new(DefaultBaseUrl);

    internal void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new InvalidOperationException("A Termii API key is required.");
        }

        if (!BaseUrl.IsAbsoluteUri)
        {
            throw new InvalidOperationException("The Termii base URL must be absolute.");
        }
    }
}
