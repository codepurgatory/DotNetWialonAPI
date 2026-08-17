using System.Text.Json;
using Wialon.Sdk.Http;
using Wialon.Sdk.Models;

namespace Wialon.Sdk.Services;

/// <summary>
/// Сервис для работы с сообщениями (messages/*).
/// </summary>
public sealed class MessagesService : WialonServiceBase
{
    public MessagesService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Загрузка сообщений за интервал (messages/load_interval).
    /// </summary>
    /// <param name="itemId">ID объекта.</param>
    /// <param name="timeFrom">Начало интервала (Unix UTC).</param>
    /// <param name="timeTo">Конец интервала (Unix UTC).</param>
    /// <param name="flags">Флаги типов сообщений (0=все данные).</param>
    /// <param name="flagMask">Маска флагов (0=любые).</param>
    /// <param name="loadCount">Максимальное количество сообщений (0=все).</param>
    public async Task<MessagesResult> LoadIntervalAsync(
        long itemId,
        long timeFrom,
        long timeTo,
        int flags = 0,
        int flagMask = 0,
        int loadCount = 0,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            itemId,
            timeFrom,
            timeTo,
            flags,
            flagMask,
            loadCount
        };
        return await PostAsync<MessagesResult>("messages/load_interval", parameters, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Загрузка последних N сообщений (messages/load_last).
    /// </summary>
    /// <param name="itemId">ID объекта.</param>
    /// <param name="lastTime">Ограничение по времени (0=без ограничения).</param>
    /// <param name="lastCount">Количество последних сообщений.</param>
    /// <param name="flags">Флаги типов сообщений.</param>
    /// <param name="flagMask">Маска флагов.</param>
    /// <param name="loadCount">Максимальное количество сообщений для загрузки.</param>
    public async Task<MessagesResult> LoadLastAsync(
        long itemId,
        int lastTime = 0,
        int lastCount = 10,
        int flags = 0,
        int flagMask = 0,
        int loadCount = 0,
        CancellationToken ct = default)
    {
        var parameters = new
        {
            itemId,
            lastTime,
            lastCount,
            flags,
            flagMask,
            loadCount
        };
        return await PostAsync<MessagesResult>("messages/load_last", parameters, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Удаление сообщения (messages/delete_message).
    /// </summary>
    public async Task DeleteMessageAsync(
        long itemId,
        long messageTime,
        int messageFlags,
        CancellationToken ct = default)
    {
        await PostAsync("messages/delete_message",
            new { itemId, msgTime = messageTime, msgFlags = messageFlags }, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Очистка загрузчика сообщений (messages/unload).
    /// </summary>
    public async Task UnloadAsync(CancellationToken ct = default)
    {
        await PostAsync("messages/unload", new { }, ct).ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для работы с элементами (item/*).
/// </summary>
public sealed class ItemService : WialonServiceBase
{
    public ItemService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Переименование элемента (item/update_name).
    /// </summary>
    public async Task UpdateNameAsync(long itemId, string name, CancellationToken ct = default)
    {
        await PostAsync("item/update_name", new { itemId, name }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Удаление элемента (item/delete_item).
    /// </summary>
    public async Task DeleteItemAsync(long itemId, CancellationToken ct = default)
    {
        await PostAsync("item/delete_item", new { itemId }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Обновление произвольного поля (item/update_custom_field).
    /// </summary>
    public async Task<Models.CustomField> UpdateCustomFieldAsync(
        long itemId,
        long fieldId,
        string name,
        string value,
        CancellationToken ct = default)
    {
        return await PostAsync<Models.CustomField>("item/update_custom_field",
            new { itemId, id = fieldId, callMode = fieldId == 0 ? "create" : "update", n = name, v = value }, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Обновление произвольного свойства (item/update_custom_property).
    /// </summary>
    public async Task UpdateCustomPropertyAsync(
        long itemId,
        string name,
        string value,
        CancellationToken ct = default)
    {
        await PostAsync("item/update_custom_property",
            new { itemId, name, value }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Обновление административного поля (item/update_admin_field).
    /// </summary>
    public async Task<Models.CustomField> UpdateAdminFieldAsync(
        long itemId,
        long fieldId,
        string name,
        string value,
        CancellationToken ct = default)
    {
        return await PostAsync<Models.CustomField>("item/update_admin_field",
            new { itemId, id = fieldId, callMode = fieldId == 0 ? "create" : "update", n = name, v = value }, ct)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// Добавление записи в журнал (item/add_log_record).
    /// </summary>
    public async Task AddLogRecordAsync(
        long itemId,
        string action,
        string message,
        CancellationToken ct = default)
    {
        await PostAsync("item/add_log_record",
            new { itemId, action, message, time = DateTimeOffset.UtcNow.ToUnixTimeSeconds() }, ct)
            .ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для работы с объектами мониторинга (unit/*).
/// </summary>
public sealed class UnitService : WialonServiceBase
{
    public UnitService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Обновление флагов расчёта (unit/update_calc_flags).
    /// </summary>
    public async Task UpdateCalcFlagsAsync(long itemId, long flags, long flagsMask, CancellationToken ct = default)
    {
        await PostAsync("unit/update_calc_flags", new { itemId, flags, flagsMask }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Обновление задачи объекта (unit/update_task).
    /// </summary>
    public async Task UpdateTaskAsync(long itemId, JsonElement taskConfig, long flags, CancellationToken ct = default)
    {
        await PostAsync("unit/update_task", new { itemId, task = taskConfig, flags }, ct).ConfigureAwait(false);
    }
}

/// <summary>
/// Сервис для работы с пользователями (user/*).
/// </summary>
public sealed class UserService : WialonServiceBase
{
    public UserService(WialonHttpClient http) : base(http) { }

    /// <summary>
    /// Изменение пароля пользователя (user/update_password).
    /// </summary>
    public async Task UpdatePasswordAsync(
        long userId,
        string oldPassword,
        string newPassword,
        CancellationToken ct = default)
    {
        await PostAsync("user/update_password",
            new { userId, oldPassword, newPassword }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Обновление локали пользователя (user/update_locale).
    /// </summary>
    public async Task UpdateLocaleAsync(long userId, JsonElement localeParams, CancellationToken ct = default)
    {
        await PostAsync("user/update_locale", new { userId, @params = localeParams }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Получение локали пользователя (user/get_locale).
    /// </summary>
    public async Task<JsonElement> GetLocaleAsync(long userId, CancellationToken ct = default)
    {
        var json = await PostAsync("user/get_locale", new { userId }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }

    /// <summary>
    /// Обновление флагов пользователя (user/update_user_flags).
    /// </summary>
    public async Task UpdateUserFlagsAsync(long userId, long flags, long flagsMask, CancellationToken ct = default)
    {
        await PostAsync("user/update_user_flags",
            new { userId, flags, flagsMask }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Обновление прав доступа пользователя к элементу (user/update_item_access).
    /// </summary>
    public async Task UpdateItemAccessAsync(
        long userId,
        long itemId,
        long accessMask,
        CancellationToken ct = default)
    {
        await PostAsync("user/update_item_access",
            new { userId, itemId, accessMask }, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Получение прав доступа пользователя к элементам (user/get_items_access).
    /// </summary>
    public async Task<JsonElement> GetItemsAccessAsync(long userId, CancellationToken ct = default)
    {
        var json = await PostAsync("user/get_items_access", new { userId }, ct).ConfigureAwait(false);
        return JsonDocument.Parse(json).RootElement.Clone();
    }
}
