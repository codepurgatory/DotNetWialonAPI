namespace Wialon.Sdk.Enums;

/// <summary>
/// Типы сообщений Wialon.
/// Флаги для фильтрации при загрузке сообщений через messages/load_interval и messages/load_last.
/// </summary>
[Flags]
public enum MessageType
{
    /// <summary>Сообщение с данными (телематика).</summary>
    Data = 0x0000,
    /// <summary>SMS.</summary>
    Sms = 0x0100,
    /// <summary>Команда.</summary>
    Command = 0x0200,
    /// <summary>Журнал пользователя.</summary>
    UserLog = 0x0400,
    /// <summary>Уведомление пользователя.</summary>
    Notification = 0x0300,
    /// <summary>Биллинговое сообщение.</summary>
    Billing = 0x0500,
    /// <summary>Событие.</summary>
    Event = 0x0600,
    /// <summary>Обработка участка.</summary>
    Segment = 0x0700,
    /// <summary>Запись сервиса WDC.</summary>
    WdcRecord = 0x0800,
    /// <summary>SMS от водителя.</summary>
    DriverSms = 0x0900,
    /// <summary>Запись журнала.</summary>
    Log = 0x1000,
    /// <summary>Использование видео.</summary>
    VideoUsage = 0x2000,
    /// <summary>Сообщение, вызвавшее срабатывание уведомления.</summary>
    NotificationTrigger = 0x4000,
    /// <summary>Задача.</summary>
    Task = 0x5000,
}
