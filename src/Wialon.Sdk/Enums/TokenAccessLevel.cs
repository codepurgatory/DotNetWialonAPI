namespace Wialon.Sdk.Enums;

/// <summary>
/// Флаги уровней доступа токена Wialon.
/// Используется при создании/управлении токенами (token/update).
/// </summary>
public enum TokenAccessLevel : long
{
    /// <summary>Онлайн-мониторинг в реальном времени.</summary>
    OnlineMonitoring = 0x100,
    /// <summary>Просмотр большинства данных.</summary>
    ViewData = 0x200,
    /// <summary>Изменение нечувствительных данных.</summary>
    EditInsensitiveData = 0x400,
    /// <summary>Изменение особо важных данных.</summary>
    EditSensitiveData = 0x800,
    /// <summary>Изменение критичных данных и удаление сообщений.</summary>
    EditCriticalData = 0x1000,
    /// <summary>Отправка команд объектам.</summary>
    SendCommands = 0x2000,
    /// <summary>Неограниченный доступ (как авторизованный пользователь).</summary>
    Unlimited = -1,
}

/// <summary>
/// Флаги ресурсов (avl_resource).
/// </summary>
[Flags]
public enum ResourceFlag : long
{
    None = 0,
    Base = 0x00000001,
    CustomProperties = 0x00000002,
    Billing = 0x00000004,
    CustomFields = 0x00000008,
    Messages = 0x00000020,
    Guid = 0x00000040,
    AdminFields = 0x00000080,
    Drivers = 0x00000100,
    Jobs = 0x00000200,
    Notifications = 0x00000400,
    Pois = 0x00000800,
    GeoZones = 0x00001000,
    ReportTemplates = 0x00002000,
    DriverAutoBinding = 0x00004000,
    DriverGroups = 0x00008000,
    Trailers = 0x00010000,
    TrailerGroups = 0x00020000,
    TrailerAutoBinding = 0x00040000,
    Orders = 0x00080000,
    GeoZoneGroups = 0x00100000,
    Tags = 0x00200000,
    TagAutoBinding = 0x00400000,
    TagGroups = 0x00800000,
    All = 0x3FFFFFFFFFFFFFFF,
}

/// <summary>
/// Флаги пользователей.
/// </summary>
[Flags]
public enum UserFlag : long
{
    None = 0,
    Base = 0x00000001,
    CustomProperties = 0x00000002,
    Billing = 0x00000004,
    CustomFields = 0x00000008,
    Messages = 0x00000020,
    Guid = 0x00000040,
    AdminFields = 0x00000080,
    OtherProperties = 0x00000100,
    Notifications = 0x00000200,
    ConnectionSettings = 0x00000400,
    MobileApps = 0x00000800,
    All = 0x3FFFFFFFFFFFFFFF,
}
