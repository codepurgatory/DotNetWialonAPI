using System.Text.Json;
using Wialon.Sdk.Http;
using Wialon.Sdk.Models;

namespace Wialon.Sdk.Services;

/// <summary>
/// Сервис для работы с ресурсами (resource/*).
/// Геозоны, водители, прицепы, уведомления, задания, POI.
/// </summary>
public sealed class ResourceService : WialonServiceBase
{
    public ResourceService(WialonHttpClient http) : base(http) { }

    /// <summary>Получение данных геозоны (resource/get_zone_data).</summary>
    public async Task<JsonElement> GetZoneDataAsync(long resourceId, long zoneId, CancellationToken ct = default)
    {
        var json = await PostAsync("resource/get_zone_data",
            new { itemId = resourceId, id = zoneId }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Получение геозон, в которых находится точка (resource/get_zones_by_point).
    /// </summary>
    public async Task<List<long>> GetZonesByPointAsync(
        long resourceId,
        double lat,
        double lon,
        CancellationToken ct = default)
    {
        return await PostAsync<List<long>>("resource/get_zones_by_point",
            new { itemId = resourceId, lat, lon }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Создание/обновление геозоны (resource/update_zone).
    /// </summary>
    public async Task<JsonElement> UpdateZoneAsync(
        long resourceId,
        long zoneId,
        string callMode,
        JsonElement zoneData,
        CancellationToken ct = default)
    {
        var json = await PostAsync("resource/update_zone",
            new { itemId = resourceId, id = zoneId, callMode, w = zoneData }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Создание/обновление водителя (resource/update_driver).
    /// </summary>
    public async Task<Driver> UpdateDriverAsync(
        long resourceId,
        long driverId,
        string callMode,
        string name,
        string code,
        string? phone = null,
        string? description = null,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            itemId = resourceId,
            id = driverId,
            callMode,
            n = name,
            c = code,
            p = phone ?? string.Empty,
            ds = description ?? string.Empty
        };
        return await PostAsync<Driver>("resource/update_driver", parameters, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Привязка водителя к объекту (resource/bind_unit_driver).
    /// </summary>
    public async Task BindUnitDriverAsync(
        long resourceId,
        long driverId,
        long unitId,
        long bindTime,
        CancellationToken ct = default)
    {
        await PostAsync("resource/bind_unit_driver",
            new { itemId = resourceId, driverId, unitId, time = bindTime }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Получение привязок водителей (resource/get_driver_bindings).
    /// </summary>
    public async Task<JsonElement> GetDriverBindingsAsync(
        long resourceId,
        long driverId,
        CancellationToken ct = default)
    {
        var json = await PostAsync("resource/get_driver_bindings",
            new { itemId = resourceId, driverId }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Создание/обновление уведомления (resource/update_notification).
    /// </summary>
    public async Task<JsonElement> UpdateNotificationAsync(
        long resourceId,
        long notificationId,
        string callMode,
        JsonElement notificationData,
        CancellationToken ct = default)
    {
        var json = await PostAsync("resource/update_notification",
            new { itemId = resourceId, id = notificationId, callMode, n = notificationData }, ct)
            .ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Создание/обновление задания (resource/update_job).
    /// </summary>
    public async Task<Job> UpdateJobAsync(
        long resourceId,
        long jobId,
        string callMode,
        JsonElement jobData,
        CancellationToken ct = default)
    {
        return await PostAsync<Job>("resource/update_job",
            new { itemId = resourceId, id = jobId, callMode, j = jobData }, ct).ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для управления токенами (token/*).
/// </summary>
public sealed class TokenService : WialonServiceBase
{
    public TokenService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Получение списка токенов (token/list).
    /// </summary>
    public async Task<List<WialonToken>> ListAsync(
        long? userId = null,
        CancellationToken ct = default)
    {
        return await PostAsync<List<WialonToken>>("token/list",
            userId.HasValue ? new { userId } : (object?)null, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Создание токена (token/update callMode=create).
    /// </summary>
    public async Task<WialonToken> CreateAsync(
        string appName,
        long activationTime = 0,
        long duration = 0,
        long accessFlags = -1,
        string? customParams = null,
        IEnumerable<long>? items = null,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            callMode = "create",
            app = appName,
            at = activationTime,
            dur = duration,
            fl = accessFlags,
            p = customParams ?? "{}",
            items = items?.ToArray() ?? Array.Empty<long>(),
        };
        return await PostAsync<WialonToken>("token/update", parameters, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Удаление токена (token/update callMode=delete).
    /// </summary>
    public async Task DeleteAsync(string tokenHash, CancellationToken ct = default)
    {
        await PostAsync("token/update",
            new { callMode = "delete", h = tokenHash }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Удаление всех токенов пользователя (token/update callMode=delete deleteAll=true).
    /// </summary>
    public async Task DeleteAllAsync(CancellationToken ct = default)
    {
        await PostAsync("token/update",
            new { callMode = "delete", deleteAll = true }, ct).ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для работы с отчётами (report/*).
/// </summary>
public sealed class ReportService : WialonServiceBase
{
    public ReportService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Запуск отчёта (report/exec_report).
    /// </summary>
    public async Task<ReportResult> ExecReportAsync(
        long resourceId,
        long reportTemplateId,
        long objectId,
        long objectSecondId,
        long intervalFrom,
        long intervalTo,
        JsonElement? reportParams = null,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            reportResourceId = resourceId,
            reportTemplateId,
            reportObjectId = objectId,
            reportObjectSecId = objectSecondId,
            interval = new { from = intervalFrom, to = intervalTo, flags = 0 },
            reportParams = reportParams ?? JsonDocument.Parse("{}").RootElement,
        };
        return await PostAsync<ReportResult>("report/exec_report", parameters, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Получение статуса выполнения отчёта (report/get_report_status).
    /// </summary>
    public async Task<JsonElement> GetReportStatusAsync(CancellationToken ct = default)
    {
        var json = await PostAsync("report/get_report_status", new { }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Получение таблиц отчёта (report/get_report_tables).
    /// </summary>
    public async Task<List<ReportTable>> GetReportTablesAsync(CancellationToken ct = default)
    {
        return await PostAsync<List<ReportTable>>("report/get_report_tables", new { }, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Получение строк таблицы отчёта (report/get_result_rows).
    /// </summary>
    public async Task<List<ReportRow>> GetResultRowsAsync(
        int tableIndex,
        int rowFrom,
        int rowTo,
        CancellationToken ct = default)
    {
        return await PostAsync<List<ReportRow>>("report/get_result_rows",
            new { tableIndex, indexFrom = rowFrom, indexTo = rowTo }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Очистка результатов отчёта (report/cleanup_result).
    /// </summary>
    public async Task CleanupResultAsync(CancellationToken ct = default)
    {
        await PostAsync("report/cleanup_result", new { }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Экспорт результатов отчёта (report/export_result).
    /// </summary>
    public async Task<string> ExportResultAsync(
        int format = 0,
        string? outputFileName = null,
        CancellationToken ct = default)
    {
        return await PostAsync("report/export_result",
            new { format, outputFileName = outputFileName ?? "report" }, ct).ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для работы с событиями в реальном времени (events/*).
/// </summary>
public sealed class EventsService : WialonServiceBase
{
    public EventsService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Загрузка событий за интервал (events/load).
    /// </summary>
    public async Task<JsonElement> LoadAsync(
        long itemId,
        long timeFrom,
        long timeTo,
        JsonElement selector,
        CancellationToken ct = default)
    {
        var json = await PostAsync("events/load",
            new { itemId, timeFrom, timeTo, sl = selector }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Получение последнего состояния событий (events/get_last).
    /// </summary>
    public async Task<JsonElement> GetLastAsync(
        long itemId,
        JsonElement selector,
        long flags,
        CancellationToken ct = default)
    {
        var json = await PostAsync("events/get_last",
            new { itemId, sl = selector, flags }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Проверка обновлений событий (events/check_updates).
    /// </summary>
    public async Task<JsonElement> CheckUpdatesAsync(long flags = 0, CancellationToken ct = default)
    {
        var json = await PostAsync("events/check_updates", new { flags }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Выгрузка событий (events/unload).
    /// </summary>
    public async Task UnloadAsync(long itemId, CancellationToken ct = default)
    {
        await PostAsync("events/unload", new { itemId }, ct).ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для работы с ретрансляторами (retranslator/*).
/// </summary>
public sealed class RetranslatorService : WialonServiceBase
{
    public RetranslatorService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Получение статистики ретранслятора (retranslator/get_stats).
    /// </summary>
    public async Task<JsonElement> GetStatsAsync(long itemId, CancellationToken ct = default)
    {
        var json = await PostAsync("retranslator/get_stats", new { itemId }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Получение списка ретрансляторов (retranslator/list).
    /// </summary>
    public async Task<JsonElement> ListAsync(long flags = 1, CancellationToken ct = default)
    {
        var json = await PostAsync("retranslator/list", new { flags }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Обновление конфигурации ретранслятора (retranslator/update_config).
    /// </summary>
    public async Task<JsonElement> UpdateConfigAsync(long itemId, JsonElement config, CancellationToken ct = default)
    {
        var json = await PostAsync("retranslator/update_config",
            new { itemId, config }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }
}

/// <summary>
/// Сервис для работы с файлами (file/*).
/// </summary>
public sealed class FileService : WialonServiceBase
{
    public FileService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Получение информации о файлах (file/list).
    /// </summary>
    public async Task<JsonElement> ListAsync(
        long itemId,
        string path = "/",
        long flags = 0,
        int maxCount = 100,
        CancellationToken ct = default)
    {
        var json = await PostAsync("file/list",
            new { itemId, path, flags, maxCount }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Создание папки (file/mkdir).
    /// </summary>
    public async Task MkdirAsync(long itemId, string path, CancellationToken ct = default)
    {
        await PostAsync("file/mkdir", new { itemId, path }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Удаление файла/папки (file/rm).
    /// </summary>
    public async Task RemoveAsync(long itemId, string path, CancellationToken ct = default)
    {
        await PostAsync("file/rm", new { itemId, path }, ct).ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для импорта/экспорта данных (exchange/*).
/// </summary>
public sealed class ExchangeService : WialonServiceBase
{
    public ExchangeService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Экспорт объекта в JSON (exchange/export_json).
    /// </summary>
    public async Task<string> ExportJsonAsync(
        long itemId,
        long flags,
        CancellationToken ct = default)
    {
        return await PostAsync("exchange/export_json",
            new { itemId, flags }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Экспорт геозон (exchange/export_zones).
    /// </summary>
    public async Task<string> ExportZonesAsync(
        long resourceId,
        IEnumerable<long> zoneIds,
        int format = 0,
        CancellationToken ct = default)
    {
        return await PostAsync("exchange/export_zones",
            new { itemId = resourceId, zoneIds = zoneIds.ToArray(), format }, ct).ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для рендеринга карты (render/*).
/// </summary>
public sealed class RenderService : WialonServiceBase
{
    public RenderService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Установка локали карты (render/set_locale).
    /// </summary>
    public async Task SetLocaleAsync(JsonElement localeParams, CancellationToken ct = default)
    {
        await PostAsync("render/set_locale", new { @params = localeParams }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Создание слоя сообщений на карте (render/create_messages_layer).
    /// </summary>
    public async Task<JsonElement> CreateMessagesLayerAsync(
        long itemId,
        long timeFrom,
        long timeTo,
        long flags,
        CancellationToken ct = default)
    {
        var json = await PostAsync("render/create_messages_layer",
            new { layerName = $"ml_{itemId}", itemId, timeFrom, timeTo, flags }, ct)
            .ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Удаление всех слоёв карты (render/remove_all_layers).
    /// </summary>
    public async Task RemoveAllLayersAsync(CancellationToken ct = default)
    {
        await PostAsync("render/remove_all_layers", new { }, ct).ConfigureAwait(false);
    }
}
