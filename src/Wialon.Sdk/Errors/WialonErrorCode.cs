namespace Wialon.Sdk.Errors;

/// <summary>
/// Коды ошибок Wialon Remote API.
/// Документация: https://sdk.wialon.com/wiki/en/sidebar/remoteapi/apiref/core/errors
/// </summary>
public enum WialonErrorCode
{
    /// <summary>Успешная операция.</summary>
    Success = 0,

    /// <summary>Невалидная сессия.</summary>
    InvalidSession = 1,

    /// <summary>Неверное имя API-сервиса. Нет сообщений за выбранный интервал.</summary>
    InvalidServiceName = 2,

    /// <summary>Неверный результат / элемент не найден.</summary>
    InvalidResult = 3,

    /// <summary>Неверный ввод / невалидная сессия / элемент не найден.</summary>
    InvalidInput = 4,

    /// <summary>Ошибка выполнения запроса.</summary>
    ExecutionError = 5,

    /// <summary>Неизвестная ошибка / сервер авторизации недоступен.</summary>
    UnknownError = 6,

    /// <summary>Доступ запрещён / пользователь отключён / неверные учётные данные.</summary>
    AccessDenied = 7,

    /// <summary>Неверное имя пользователя или пароль.</summary>
    InvalidCredentials = 8,

    /// <summary>Сервер авторизации недоступен.</summary>
    AuthServerUnavailable = 9,

    /// <summary>Достигнут лимит одновременных запросов.</summary>
    ConcurrentRequestLimit = 10,

    /// <summary>Ошибка сброса пароля.</summary>
    PasswordResetError = 11,

    /// <summary>Агро-подсистема не загружена.</summary>
    AgroSubsystemNotLoaded = 12,

    /// <summary>Ошибка биллинга.</summary>
    BillingError = 14,

    /// <summary>Нет сообщений за выбранный интервал.</summary>
    NoMessages = 1001,

    /// <summary>Элемент с таким уникальным свойством уже существует / ограничение тарифа.</summary>
    DuplicateOrQuotaLimit = 1002,

    /// <summary>Разрешён только один запрос / превышен лимит.</summary>
    SingleRequestAllowed = 1003,

    /// <summary>Превышен лимит сообщений.</summary>
    MessageLimitExceeded = 1004,

    /// <summary>Время выполнения превысило лимит.</summary>
    ExecutionTimeout = 1005,

    /// <summary>Превышен лимит попыток ввода кода двухфакторной авторизации.</summary>
    TwoFactorAttemptsLimit = 1006,

    /// <summary>IP изменился или сессия истекла.</summary>
    IpChangedOrSessionExpired = 1011,

    /// <summary>Неверный элемент или целевой ресурс.</summary>
    InvalidItemOrTargetResource = 2001,

    /// <summary>Целевой ресурс не является учётной записью.</summary>
    TargetNotAccount = 2002,

    /// <summary>Неверный целевой плагин.</summary>
    InvalidTargetPlugin = 2003,

    /// <summary>Целевая учётная запись заблокирована.</summary>
    TargetAccountBlocked = 2004,

    /// <summary>Неверный создатель целевого ресурса.</summary>
    InvalidTargetCreator = 2005,

    /// <summary>Нет доступа к элементу для целевого создателя.</summary>
    NoAccessForTargetCreator = 2006,

    /// <summary>Неверный исходный ресурс.</summary>
    InvalidSourceResource = 2007,

    /// <summary>Элемент уже находится в целевом ресурсе.</summary>
    ItemAlreadyInTargetResource = 2008,

    /// <summary>Целевой ресурс принадлежит другому пользователю верхнего уровня.</summary>
    DifferentTopLevelUser = 2009,

    /// <summary>Недостаточно счётчика ресурсов элементов в target_resource.</summary>
    InsufficientResourceQuota = 2010,

    /// <summary>Неверный плагин элемента.</summary>
    InvalidItemPlugin = 2011,

    /// <summary>Ошибка изменения тарифицируемого элемента учётной записи.</summary>
    BilledItemChangeError = 2012,

    /// <summary>Ошибка изменения создателя элемента.</summary>
    CreatorChangeError = 2013,

    /// <summary>Пользователь является создателем элементов системы.</summary>
    UserIsCreatorOfItems = 2014,

    /// <summary>Удаление датчика запрещено, так как он используется в другом датчике.</summary>
    SensorInUse = 2015,

    /// <summary>Внутренняя ошибка (таймаут сети).</summary>
    InternalNetworkTimeout = -100,

    /// <summary>Внутренняя ошибка (неверный ответ сети).</summary>
    InternalNetworkResponse = -101,
}
