using System.Text.Json;
using System.Text.Json.Serialization;
using Wialon.Sdk.Enums;
using Wialon.Sdk.Http;
using Wialon.Sdk.Models;

namespace Wialon.Sdk.Services;

/// <summary>
/// Сервис для основных методов Wialon API (core/*).
/// </summary>
public sealed class CoreService : WialonServiceBase
{
    public CoreService(WialonHttpClient http) : base(http) { }

    // -----------------------------------------------------------------------
    // Поиск элементов
    // -----------------------------------------------------------------------

    /// <summary>
    /// Поиск элемента по ID (core/search_item).
    /// </summary>
    /// <param name="id">ID элемента.</param>
    /// <param name="flags">Флаги данных (какие поля вернуть).</param>
    public async Task<JsonElement> SearchItemAsync(long id, long flags, CancellationToken ct = default)
    {
        var result = await PostAsync<SearchItemResponse>("core/search_item",
            new { id, flags }, ct).ConfigureAwait(false);
        return result.Item;
    }

    /// <summary>
    /// Поиск объекта (avl_unit) по ID с указанными флагами.
    /// </summary>
    public async Task<Unit?> SearchUnitAsync(long id, long flags = 1025, CancellationToken ct = default)
    {
        var json = await PostAsync("core/search_item", new { id, flags }, ct).ConfigureAwait(false);
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("item", out var itemEl))
            return JsonSerializer.Deserialize<Unit>(itemEl.GetRawText());
        return null;
    }

    /// <summary>
    /// Поиск элементов по параметрам (core/search_items).
    /// </summary>
    public async Task<SearchResult<JsonElement>> SearchItemsAsync(
        string itemsType,
        string propName = "sys_name",
        string propValueMask = "*",
        string sortType = "sys_name",
        long flags = 1,
        int from = 0,
        int to = 0,
        int force = 1,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            spec = new
            {
                itemsType,
                propName,
                propValueMask,
                sortType,
            },
            force,
            flags,
            from,
            to
        };
        return await PostAsync<SearchResult<JsonElement>>("core/search_items", parameters, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Поиск объектов (avl_unit) по маске имени.
    /// </summary>
    public async Task<SearchResult<Unit>> SearchUnitsAsync(
        string nameMask = "*",
        long flags = 0x441,
        int from = 0,
        int to = 0,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            spec = new
            {
                itemsType = "avl_unit",
                propName = "sys_name",
                propValueMask = nameMask,
                sortType = "sys_name",
            },
            force = 1,
            flags,
            from,
            to
        };
        return await PostAsync<SearchResult<Unit>>("core/search_items", parameters, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Поиск пользователей по маске имени.
    /// </summary>
    public async Task<SearchResult<WialonUser>> SearchUsersAsync(
        string nameMask = "*",
        long flags = 1,
        int from = 0,
        int to = 0,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            spec = new
            {
                itemsType = "user",
                propName = "sys_name",
                propValueMask = nameMask,
                sortType = "sys_name",
            },
            force = 1,
            flags,
            from,
            to
        };
        return await PostAsync<SearchResult<WialonUser>>("core/search_items", parameters, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Поиск ресурсов по маске имени.
    /// </summary>
    public async Task<SearchResult<Resource>> SearchResourcesAsync(
        string nameMask = "*",
        long flags = 1,
        int from = 0,
        int to = 0,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            spec = new
            {
                itemsType = "avl_resource",
                propName = "sys_name",
                propValueMask = nameMask,
                sortType = "sys_name",
            },
            force = 1,
            flags,
            from,
            to
        };
        return await PostAsync<SearchResult<Resource>>("core/search_items", parameters, ct)
            .ConfigureAwait(false);
    }

    // -----------------------------------------------------------------------
    // Пакетные запросы
    // -----------------------------------------------------------------------

    /// <summary>
    /// Выполнение нескольких запросов в одном (core/batch).
    /// </summary>
    /// <param name="requests">Список запросов (svc + params).</param>
    /// <param name="stopOnError">Остановить при первой ошибке.</param>
    public async Task<List<JsonElement>> BatchAsync(
        IEnumerable<BatchRequest> requests,
        bool stopOnError = false,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            @params = requests.Select(r => new { svc = r.Svc, @params = r.Params }),
            flags = stopOnError ? 1 : 0
        };
        return await PostAsync<List<JsonElement>>("core/batch", parameters, ct)
            .ConfigureAwait(false);
    }

    // -----------------------------------------------------------------------
    // Вспомогательные
    // -----------------------------------------------------------------------

    /// <summary>
    /// Проверка уникальности имени элемента (core/check_unique).
    /// </summary>
    /// <returns>true если имя уникально.</returns>
    public async Task<bool> CheckUniqueAsync(string type, string value, CancellationToken ct = default)
    {
        var json = await PostAsync("core/check_unique", new { type, value }, ct).ConfigureAwait(false);
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.TryGetProperty("result", out var r) && r.GetInt32() == 0;
    }

    /// <summary>
    /// Создание объекта мониторинга (core/create_unit).
    /// </summary>
    public async Task<Unit> CreateUnitAsync(
        string name,
        long creatorId,
        long dataFlags = 1,
        CancellationToken ct = default)
    {
        var json = await PostAsync("core/create_unit",
            new { creatorId, name, hwTypeId = 0, dataFlags }, ct).ConfigureAwait(false);
        return JsonSerializer.Deserialize<Unit>(json)
               ?? throw new InvalidOperationException("Failed to deserialize created unit.");
    }

    /// <summary>
    /// Обновление флагов данных для подписки на обновления (core/update_data_flags).
    /// </summary>
    public async Task UpdateDataFlagsAsync(
        string type,
        IEnumerable<long> itemIds,
        long flags,
        CancellationToken ct = default)
    {
        var spec = itemIds.Select(id => new { type, data = id, flags }).ToList();
        await PostAsync("core/update_data_flags", new { spec }, ct).ConfigureAwait(false);
    }

    // -----------------------------------------------------------------------
    // Внутренние типы
    // -----------------------------------------------------------------------

    private sealed class SearchItemResponse
    {
        [JsonPropertyName("item")]
        public JsonElement Item { get; set; }

        [JsonPropertyName("flags")]
        public long Flags { get; set; }
    }
}

/// <summary>Запрос для batch-метода.</summary>
public sealed class BatchRequest
{
    public string Svc { get; set; } = string.Empty;
    public object? Params { get; set; }
}
