using Wialon.Sdk.Auth;
using Wialon.Sdk.Http;
using Wialon.Sdk.Models;
using Wialon.Sdk.Services;

namespace Wialon.Sdk;

/// <summary>
/// Главный клиент Wialon API SDK.
/// Точка входа для всех операций с Wialon Remote API.
/// </summary>
/// <example>
/// <code>
/// var client = new WialonClient(new WialonClientOptions
/// {
///     Host = "https://hst-api.wialon.com",
///     AccessToken = "your_72_char_token"
/// });
/// var session = await client.LoginAsync();
/// var units = await client.Core.SearchUnitsAsync("*");
/// </code>
/// </example>
public sealed class WialonClient : IDisposable
{
    private readonly WialonHttpClient _http;
    private readonly TokenAuth _auth;
    private readonly WialonClientOptions _options;
    private bool _disposed;

    /// <summary>Текущая сессия. null до вызова LoginAsync().</summary>
    public Session? CurrentSession { get; private set; }

    /// <summary>ID текущей сессии (sid). null до вызова LoginAsync().</summary>
    public string? SessionId => CurrentSession?.Eid;

    // ---- Сервисы ----
    public CoreService Core { get; }
    public ItemService Items { get; }
    public UnitService Units { get; }
    public UserService Users { get; }
    public ResourceService Resources { get; }
    public MessagesService Messages { get; }
    public ReportService Reports { get; }
    public EventsService Events { get; }
    public TokenService Tokens { get; }
    public RetranslatorService Retranslators { get; }
    public FileService Files { get; }
    public ExchangeService Exchange { get; }
    public RenderService Render { get; }

    private readonly List<WialonServiceBase> _allServices;

    /// <summary>
    /// Создаёт клиент с указанными настройками.
    /// </summary>
    public WialonClient(WialonClientOptions options, System.Net.Http.HttpClient? httpClient = null)
    {
        _options = options;
        _http = new WialonHttpClient(options, httpClient);
        _auth = new TokenAuth(_http);

        Core          = new CoreService(_http);
        Items         = new ItemService(_http);
        Units         = new UnitService(_http);
        Users         = new UserService(_http);
        Resources     = new ResourceService(_http);
        Messages      = new MessagesService(_http);
        Reports       = new ReportService(_http);
        Events        = new EventsService(_http);
        Tokens        = new TokenService(_http);
        Retranslators = new RetranslatorService(_http);
        Files         = new FileService(_http);
        Exchange      = new ExchangeService(_http);
        Render        = new RenderService(_http);

        _allServices = new List<WialonServiceBase>
        {
            Core, Items, Units, Users, Resources, Messages, Reports,
            Events, Tokens, Retranslators, Files, Exchange, Render
        };
    }

    /// <summary>
    /// Создаёт клиент, загружая настройки из переменных окружения.
    /// Требует: WIALON_ACCESS_TOKEN и (опционально) WIALON_API_HOST.
    /// </summary>
    public static WialonClient FromEnvironment()
    {
        var token = Environment.GetEnvironmentVariable("WIALON_ACCESS_TOKEN")
                    ?? throw new InvalidOperationException(
                        "WIALON_ACCESS_TOKEN environment variable is not set.");
        var host = Environment.GetEnvironmentVariable("WIALON_API_HOST")
                   ?? "https://hst-api.wialon.com";

        return new WialonClient(new WialonClientOptions
        {
            AccessToken = token,
            Host = host
        });
    }

    /// <summary>
    /// Выполняет авторизацию через токен доступа.
    /// После успешного логина все сервисы готовы к работе.
    /// </summary>
    /// <param name="operateAs">Опциональный логин подпользователя для операций от его имени.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>Информация о сессии.</returns>
    public async Task<Session> LoginAsync(string? operateAs = null, CancellationToken ct = default)
    {
        var session = await _auth.LoginAsync(_options.AccessToken, operateAs, ct).ConfigureAwait(false);
        CurrentSession = session;

        // Устанавливаем sid для всех сервисов
        foreach (var svc in _allServices)
            svc.SessionId = session.Eid;

        return session;
    }

    /// <summary>
    /// Выполняет выход из системы и очищает сессию.
    /// </summary>
    public async Task LogoutAsync(CancellationToken ct = default)
    {
        if (CurrentSession is null) return;

        await _auth.LogoutAsync(CurrentSession.Eid, ct).ConfigureAwait(false);
        CurrentSession = null;

        foreach (var svc in _allServices)
            svc.SessionId = null;
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
