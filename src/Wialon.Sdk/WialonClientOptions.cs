namespace Wialon.Sdk;

/// <summary>
/// Конфигурация Wialon API клиента.
/// </summary>
public sealed class WialonClientOptions
{
    /// <summary>
    /// Базовый URL хоста Wialon.
    /// Для Wialon Hosting: https://hst-api.wialon.com
    /// Для Wialon Local: URL вашего сервера мониторинга.
    /// </summary>
    public string Host { get; set; } = "https://hst-api.wialon.com";

    /// <summary>
    /// 72-символьный токен доступа.
    /// Получить: https://&lt;host&gt;/login.html?client_id=MyApp&amp;access_type=-1&amp;duration=0
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>Таймаут HTTP-запросов (по умолчанию 30 секунд).</summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Количество повторных попыток при ошибке 1003 (лимит запросов).
    /// По умолчанию 3.
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>Задержка между повторными попытками при ошибке 1003.</summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Нормализует хост: убирает завершающий слеш.
    /// </summary>
    internal string NormalizedHost => Host.TrimEnd('/');
}
