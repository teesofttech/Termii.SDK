using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text;
using System.Text.Json;

namespace Termii;

internal sealed class TermiiJsonHttpPipeline
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    private readonly HttpClient _httpClient;
    private readonly TermiiOptions _options;

    public TermiiJsonHttpPipeline(HttpClient httpClient, TermiiOptions options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<HttpResponseMessage> SendAsync(
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

        if (!path.StartsWith("/", StringComparison.Ordinal))
        {
            throw new ArgumentException("Termii request paths must start with '/'.", nameof(path));
        }

        if (Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var uri) && uri.IsAbsoluteUri)
        {
            throw new ArgumentException("Termii request paths must be relative.", nameof(path));
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

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        var statusCode = response.StatusCode;
        var rawResponseBody = response.Content is null
            ? string.Empty
            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var error = TermiiApiError.Parse(rawResponseBody);

        response.Dispose();

        throw new TermiiApiException(statusCode, error.Message, error.Code, rawResponseBody);
    }

    public async Task<TResponse> SendJsonAsync<TResponse>(
        HttpMethod method,
        string path,
        object? body,
        TermiiAuthenticationLocation authenticationLocation,
        CancellationToken cancellationToken)
    {
        using var response = await SendAsync(method, path, body, authenticationLocation, cancellationToken)
            .ConfigureAwait(false);

        if (response.Content is null)
        {
            throw new InvalidOperationException("The Termii API returned an empty response.");
        }

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var value = JsonSerializer.Deserialize<TResponse>(json, JsonSerializerOptions);

        if (value is null)
        {
            throw new InvalidOperationException("The Termii API returned an empty response.");
        }

        return value;
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
