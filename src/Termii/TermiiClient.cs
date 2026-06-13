namespace Termii;

/// <summary>
/// Main entry point for the Termii SDK.
/// </summary>
public sealed class TermiiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private readonly TermiiJsonHttpPipeline _pipeline;

    public TermiiClient(TermiiOptions options)
        : this(CreateDefaultHttpClient(options), options, ownsHttpClient: true)
    {
    }

    public TermiiClient(HttpClient httpClient, TermiiOptions options)
        : this(httpClient, options, ownsHttpClient: false)
    {
    }

    private TermiiClient(HttpClient httpClient, TermiiOptions options, bool ownsHttpClient)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        Options.Validate();

        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _ownsHttpClient = ownsHttpClient;
        _httpClient.BaseAddress ??= Options.BaseUrl;

        if (_ownsHttpClient)
        {
            _httpClient.Timeout = Options.Timeout;
        }

        _pipeline = new TermiiJsonHttpPipeline(_httpClient, Options);
    }

    public TermiiOptions Options { get; }

    public void Dispose()
    {
        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
    }

    internal Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string path,
        object? body = null,
        TermiiAuthenticationLocation authenticationLocation = TermiiAuthenticationLocation.Query,
        CancellationToken cancellationToken = default)
    {
        return _pipeline.SendAsync(method, path, body, authenticationLocation, cancellationToken);
    }

    private static HttpClient CreateDefaultHttpClient(TermiiOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        return new HttpClient
        {
            BaseAddress = options.BaseUrl,
        };
    }
}
