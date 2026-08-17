namespace Wialon.Sdk.Enums;

/// <summary>
/// Флаги прав доступа (ACL) к элементам Wialon.
/// Используется в core/check_items_billing, user/update_item_access и других методах.
/// </summary>
[Flags]
public enum AccessFlag : long
{
    None = 0,

    // ---- Стандартные (любые элементы) ----
    /// <summary>Просмотр элемента и его основных свойств.</summary>
    View = 0x0001,
    /// <summary>Просмотр подробных свойств.</summary>
    ViewDetailed = 0x0002,
    /// <summary>Управление доступом к элементу.</summary>
    ManageAccess = 0x0004,
    /// <summary>Удаление элемента.</summary>
    Delete = 0x0008,
    /// <summary>Переименование элемента.</summary>
    Rename = 0x0010,
    /// <summary>Просмотр произвольных полей.</summary>
    ViewCustomFields = 0x0020,
    /// <summary>Управление произвольными полями.</summary>
    ManageCustomFields = 0x0040,
    /// <summary>Редактирование неупомянутых свойств.</summary>
    EditOtherProperties = 0x0080,
    /// <summary>Изменение иконки.</summary>
    ChangeIcon = 0x0100,
    /// <summary>Запрос сообщений и отчётов.</summary>
    QueryMessages = 0x0200,
    /// <summary>Редактирование рекурсивных элементов.</summary>
    EditRecursive = 0x0400,
    /// <summary>Управление журналом.</summary>
    ManageLog = 0x0800,
    /// <summary>Просмотр административных полей.</summary>
    ViewAdminFields = 0x1000,
    /// <summary>Управление административными полями.</summary>
    ManageAdminFields = 0x2000,
    /// <summary>Просмотр и скачивание файлов.</summary>
    ViewFiles = 0x4000,
    /// <summary>Загрузка и удаление файлов.</summary>
    ManageFiles = 0x8000,

    // ---- Объекты и группы объектов ----
    /// <summary>Редактирование настроек подключения.</summary>
    EditConnectionSettings = 0x0000100000,
    /// <summary>Создание, редактирование и удаление датчиков.</summary>
    ManageSensors = 0x0000200000,
    /// <summary>Редактирование счётчиков.</summary>
    EditCounters = 0x0000400000,
    /// <summary>Удаление сообщений.</summary>
    DeleteMessages = 0x0000800000,
    /// <summary>Отправка команд.</summary>
    SendCommands = 0x0001000000,
    /// <summary>Управление событиями.</summary>
    ManageEvents = 0x0002000000,
    /// <summary>Просмотр настроек подключения.</summary>
    ViewConnectionSettings = 0x0004000000,
    /// <summary>Просмотр интервалов техобслуживания.</summary>
    ViewMaintenance = 0x0010000000,
    /// <summary>Создание, редактирование и удаление интервалов техобслуживания.</summary>
    ManageMaintenance = 0x0020000000,
    /// <summary>Импорт сообщений.</summary>
    ImportMessages = 0x0040000000,
    /// <summary>Экспорт сообщений.</summary>
    ExportMessages = 0x0080000000,
    /// <summary>Просмотр команд.</summary>
    ViewCommands = 0x0400000000,
    /// <summary>Создание, редактирование и удаление команд.</summary>
    ManageCommands = 0x0800000000,
    /// <summary>Изменение детектора поездок.</summary>
    EditTripDetector = 0x4000000000,
    /// <summary>Использование объекта в уведомлениях, заданиях, маршрутах, ретрансляторах.</summary>
    UseInServices = 0x8000000000,

    // ---- Пользователи ----
    /// <summary>Управление правами доступа пользователя.</summary>
    ManageUserAccess = 0x100000,
    /// <summary>Выполнение действий от имени пользователя.</summary>
    OperateAs = 0x200000,
    /// <summary>Редактирование основных свойств пользователя.</summary>
    EditUserProperties = 0x400000,

    // ---- Ресурсы/Учётные записи ----
    /// <summary>Просмотр уведомлений.</summary>
    ViewNotifications = 0x0000000100000,
    /// <summary>Создание, редактирование и удаление уведомлений.</summary>
    ManageNotifications = 0x0000000200000,
    /// <summary>Просмотр геозон.</summary>
    ViewGeoZones = 0x0000001000000,
    /// <summary>Создание, редактирование и удаление геозон.</summary>
    ManageGeoZones = 0x0000002000000,
    /// <summary>Просмотр заданий.</summary>
    ViewJobs = 0x0000004000000,
    /// <summary>Создание, редактирование и удаление заданий.</summary>
    ManageJobs = 0x0000008000000,
    /// <summary>Просмотр шаблонов отчётов.</summary>
    ViewReportTemplates = 0x0000010000000,
    /// <summary>Создание, редактирование и удаление шаблонов отчётов.</summary>
    ManageReportTemplates = 0x0000020000000,
    /// <summary>Просмотр водителей.</summary>
    ViewDrivers = 0x0000040000000,
    /// <summary>Создание, редактирование и удаление водителей.</summary>
    ManageDrivers = 0x0000080000000,
    /// <summary>Управление учётной записью.</summary>
    ManageAccount = 0x0000100000000,
    /// <summary>Просмотр заявок.</summary>
    ViewOrders = 0x0000200000000,
    /// <summary>Создание, редактирование и удаление заявок.</summary>
    ManageOrders = 0x0000400000000,
    /// <summary>Просмотр пассажиров.</summary>
    ViewPassengers = 0x0000800000000,
    /// <summary>Создание, редактирование и удаление пассажиров.</summary>
    ManagePassengers = 0x0001000000000,
    /// <summary>Просмотр прицепов.</summary>
    ViewTrailers = 0x0100000000000,
    /// <summary>Создание, редактирование и удаление прицепов.</summary>
    ManageTrailers = 0x0200000000000,

    /// <summary>Полный доступ ко всем возможным правам.</summary>
    FullAccess = 0x7FFFFFFFFFFFFFFF,
}
