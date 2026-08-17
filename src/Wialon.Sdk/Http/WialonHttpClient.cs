using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Wialon.Sdk.Errors;

namespace Wialon.Sdk.Http;

/// <summary>
/// Низкоуровневый HTTP-клиент для Wialon Remote API.
/// Все запросы: POST, Content-Type: application/x-www-form-urlencoded.
/// URL: {host}/wialon/ajax.html?sid={sid}&svc={svc}&params={json}
/// </summary>
public sealed class WialonHttpClient : IDisposable
{
    private readonly HttpClient _http;
    private readonly WialonClientOptions _options;
    private bool _disposed;

    public WialonHttpClient(WialonClientOptions options, HttpClient? httpClient = null)
    {
        _options = options;
        _http = httpClient ?? new HttpClient();
        _http.Timeout = options.Timeout;
        _http.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Выполняет POST-запрос к Wialon API.
    /// </summary>
    /// <param name="svc">Имя сервиса (например, "core/search_items").</param>
    /// <param name="parameters">Параметры запроса (будут сериализованы в JSON).</param>
    /// <param name="sessionId">ID сессии (sid). null — только для token/login.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>JSON-строка ответа.</returns>
    public async Task<string> PostAsync(
        string svc,
        object? parameters = null,
        string? sessionId = null,
        CancellationToken ct = default)
    {
        var paramsJson = parameters is null
            ? "{}"
            : JsonSerializer.Serialize(parameters, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

        var url = $"{_options.NormalizedHost}/wialon/ajax.html";

        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("svc", svc),
            new KeyValuePair<string, string>("params", paramsJson),
            new KeyValuePair<string, string>("sid", sessionId ?? string.Empty),
        });

        for (int attempt = 0; attempt <= _options.RetryCount; attempt++)
        {
            HttpResponseMessage response;
            try
            {
                response = await _http.PostAsync(url, formContent, ct).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new WialonException(WialonErrorCode.InternalNetworkTimeout,
                    $"HTTP request failed: {ex.Message}", null);
            }

            var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

            // Retry при ошибке 1003 (лимит запросов)
            if (attempt < _options.RetryCount
                && ErrorParser.TryGetErrorCode(body, out var code)
                && code == WialonErrorCode.SingleRequestAllowed)
            {
                await Task.Delay(_options.RetryDelay, ct).ConfigureAwait(false);
                // Пересоздаём FormUrlEncodedContent для повторного запроса
                formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("svc", svc),
                    new KeyValuePair<string, string>("params", paramsJson),
                    new KeyValuePair<string, string>("sid", sessionId ?? string.Empty),
                });
                continue;
            }

            ErrorParser.ThrowIfError(body);
            return body;
        }

        throw new WialonException(WialonErrorCode.SingleRequestAllowed);
    }

    /// <summary>
    /// Выполняет POST-запрос и десериализует ответ в тип T.
    /// </summary>
    public async Task<T> PostAsync<T>(
        string svc,
        object? parameters = null,
        string? sessionId = null,
        CancellationToken ct = default)
    {
        var json = await PostAsync(svc, parameters, sessionId, ct).ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new WialonException(WialonErrorCode.InvalidResult,
            $"Failed to deserialize response to {typeof(T).Name}", json);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _http.Dispose();
            _disposed = true;
        }
    }
}
