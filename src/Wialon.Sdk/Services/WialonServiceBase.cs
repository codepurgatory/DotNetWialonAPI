using Wialon.Sdk.Http;

namespace Wialon.Sdk.Services;

/// <summary>
/// Базовый класс для всех сервисов Wialon SDK.
/// </summary>
public abstract class WialonServiceBase
{
    protected readonly WialonHttpClient Http;
    private string? _sessionId;

    protected WialonServiceBase(WialonHttpClient http)
    {
        Http = http;
    }

    /// <summary>
    /// ID текущей сессии. Устанавливается после успешного логина.
    /// </summary>
    public string? SessionId
    {
        get => _sessionId;
        internal set => _sessionId = value;
    }

    /// <summary>
    /// Проверяет наличие активной сессии и возвращает её ID.
    /// </summary>
    protected string RequireSession()
    {
        if (string.IsNullOrEmpty(_sessionId))
            throw new InvalidOperationException(
                "No active Wialon session. Call WialonClient.LoginAsync() first.");
        return _sessionId;
    }

    /// <summary>
    /// Выполняет POST-запрос с использованием текущей сессии.
    /// </summary>
    protected Task<string> PostAsync(string svc, object? parameters = null, CancellationToken ct = default)
        => Http.PostAsync(svc, parameters, RequireSession(), ct);

    /// <summary>
    /// Выполняет POST-запрос и десериализует ответ.
    /// </summary>
    protected Task<T> PostAsync<T>(string svc, object? parameters = null, CancellationToken ct = default)
        => Http.PostAsync<T>(svc, parameters, RequireSession(), ct);
}
