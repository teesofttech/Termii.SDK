using System.Net;

namespace Termii.Tests.Infrastructure;

internal sealed class TestHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();

    public TestHttpMessageHandler()
        : this(HttpStatusCode.OK, "{}")
    {
    }

    public TestHttpMessageHandler(HttpStatusCode statusCode, string responseBody)
    {
        Enqueue(statusCode, responseBody);
    }

    public IReadOnlyList<HttpRequestMessage> Requests => _requests;

    public HttpRequestMessage? LastRequest => _requests.Count == 0 ? null : _requests[^1];

    private readonly List<HttpRequestMessage> _requests = new();

    public void Enqueue(HttpStatusCode statusCode, string responseBody)
    {
        _responses.Enqueue(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseBody),
        });
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requests.Add(request);

        if (_responses.Count == 0)
        {
            throw new InvalidOperationException("No fake HTTP response was queued for the request.");
        }

        return Task.FromResult(_responses.Dequeue());
    }
}
