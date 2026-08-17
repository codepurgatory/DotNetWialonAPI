using System.Net;
using System.Net.Http;

namespace Wialon.Sdk.Tests.Fixtures;

/// <summary>
/// Mock HttpMessageHandler для подмены HTTP-ответов в тестах.
/// Позволяет тестировать без реальных HTTP-запросов.
/// </summary>
public sealed class MockHttpHandler : HttpMessageHandler
{
    private readonly Queue<(HttpStatusCode status, string body)> _responses = new();
    private readonly List<HttpRequestMessage> _requests = new();

    /// <summary>Все захваченные запросы.</summary>
    public IReadOnlyList<HttpRequestMessage> Requests => _requests;

    /// <summary>Последний захваченный запрос.</summary>
    public HttpRequestMessage? LastRequest => _requests.LastOrDefault();

    /// <summary>Количество запросов.</summary>
    public int RequestCount => _requests.Count;

    /// <summary>Добавить следующий ответ в очередь.</summary>
    public MockHttpHandler Respond(string jsonBody, HttpStatusCode status = HttpStatusCode.OK)
    {
        _responses.Enqueue((status, jsonBody));
        return this;
    }

    /// <summary>Добавить ответ с ошибкой Wialon.</summary>
    public MockHttpHandler RespondWithError(int errorCode)
        => Respond($"{{\"error\":{errorCode}}}");

    /// <summary>Добавить несколько одинаковых ответов.</summary>
    public MockHttpHandler RespondTimes(string jsonBody, int count)
    {
        for (int i = 0; i < count; i++)
            Respond(jsonBody);
        return this;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        _requests.Add(request);

        if (_responses.Count == 0)
            throw new InvalidOperationException(
                $"No more mocked responses. Request: {request.Method} {request.RequestUri}");

        var (status, body) = _responses.Dequeue();
        var response = new HttpResponseMessage(status)
        {
            Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json")
        };
        return Task.FromResult(response);
    }

    /// <summary>
    /// Создаёт WialonClient с мокированным HTTP-обработчиком.
    /// </summary>
    public static (MockHttpHandler handler, WialonClient client) CreateClient(
        string accessToken = "test_token_72chars_0000000000000000000000000000000000000000000000000000000000",
        string host = "https://test.wialon.local")
    {
        var handler = new MockHttpHandler();
        var httpClient = new System.Net.Http.HttpClient(handler);
        var options = new WialonClientOptions { AccessToken = accessToken, Host = host };
        var client = new WialonClient(options, httpClient);
        return (handler, client);
    }
}
