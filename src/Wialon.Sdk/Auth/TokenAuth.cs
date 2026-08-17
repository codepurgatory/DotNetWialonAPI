using System.Text.Json.Serialization;
using Wialon.Sdk.Http;
using Wialon.Sdk.Models;

namespace Wialon.Sdk.Auth;

/// <summary>
/// Аутентификация в Wialon через token/login.
/// Использует 72-символьный токен доступа для получения сессии.
/// </summary>
public sealed class TokenAuth
{
    private readonly WialonHttpClient _http;

    public TokenAuth(WialonHttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Выполняет вход в систему через токен доступа.
    /// </summary>
    /// <param name="accessToken">72-символьный токен доступа.</param>
    /// <param name="operateAs">Опциональный ID подпользователя для операций от его имени.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Сессия с eid (session id) и информацией о пользователе.</returns>
    public async Task<Session> LoginAsync(
        string accessToken,
        string? operateAs = null,
        CancellationToken ct = default)
    {
        var parameters = new TokenLoginParams
        {
            Token = accessToken,
            OperateAs = operateAs,
        };

        // token/login не требует sid
        return await _http.PostAsync<Session>(
            "token/login", parameters, sessionId: null, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Выполняет выход из системы.
    /// </summary>
    public async Task LogoutAsync(string sessionId, CancellationToken ct = default)
    {
        await _http.PostAsync("core/logout", new { }, sessionId, ct).ConfigureAwait(false);
    }

    private sealed class TokenLoginParams
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("operateAs")]
        public string? OperateAs { get; set; }

        [JsonPropertyName("fl")]
        public int Flags { get; set; } = 0;
    }
}
