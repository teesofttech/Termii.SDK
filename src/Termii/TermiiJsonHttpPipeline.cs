using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Termii;

internal sealed class TermiiJsonHttpPipeline
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly HttpClient _httpClient;
    private readonly TermiiOptions _options;

    public TermiiJsonHttpPipeline(HttpClient httpClient, TermiiOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string path,
        object? body,
        TermiiAuthenticationLocation authenticationLocation,
        CancellationToken cancellationToken)
    {
        if (method is null)
        {
            throw new ArgumentNullException(nameof(method));
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("A request path is required.", nameof(path));
        }

        var requestUri = authenticationLocation == TermiiAuthenticationLocation.Query
            ? AppendApiKey(path)
            : path;

        var request = new HttpRequestMessage(method, requestUri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (body is not null || authenticationLocation == TermiiAuthenticationLocation.Body)
        {
            request.Content = CreateJsonContent(body, authenticationLocation);
        }

        return _httpClient.SendAsync(request, cancellationToken);
    }

    private string AppendApiKey(string path)
    {
        var separator = path.IndexOf("?", StringComparison.Ordinal) >= 0 ? "&" : "?";

        return $"{path}{separator}api_key={Uri.EscapeDataString(_options.ApiKey)}";
    }

    private HttpContent CreateJsonContent(object? body, TermiiAuthenticationLocation authenticationLocation)
    {
        var json = authenticationLocation == TermiiAuthenticationLocation.Body
            ? SerializeBodyWithApiKey(body)
            : JsonSerializer.Serialize(body, JsonSerializerOptions);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private string SerializeBodyWithApiKey(object? body)
    {
        using var stream = new MemoryStream();

        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            writer.WriteString("api_key", _options.ApiKey);

            if (body is not null)
            {
                using var document = JsonDocument.Parse(JsonSerializer.Serialize(body, JsonSerializerOptions));

                if (document.RootElement.ValueKind != JsonValueKind.Object)
                {
                    throw new InvalidOperationException("Termii JSON request bodies must serialize to an object.");
                }

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    if (string.Equals(property.Name, "api_key", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    property.WriteTo(writer);
                }
            }

            writer.WriteEndObject();
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
