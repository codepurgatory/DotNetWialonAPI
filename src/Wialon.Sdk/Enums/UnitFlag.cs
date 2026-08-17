namespace Wialon.Sdk.Enums;

/// <summary>
/// Флаги для объектов (avl_unit). Определяют, какие данные вернёт API в ответе.
/// Используется в core/search_items параметр flags.
/// </summary>
[Flags]
public enum UnitFlag : long
{
    None = 0,
    /// <summary>Основные свойства (nm, cls, id, uacl).</summary>
    Base = 0x00000001,
    /// <summary>Пользовательские свойства (prp).</summary>
    CustomProperties = 0x00000002,
    /// <summary>Информация о биллинге (crt, bact).</summary>
    Billing = 0x00000008,
    /// <summary>Произвольные поля (flds).</summary>
    CustomFields = 0x00000008,
    /// <summary>Изображение объекта.</summary>
    Image = 0x00000010,
    /// <summary>Сообщения.</summary>
    Messages = 0x00000020,
    /// <summary>GUID.</summary>
    Guid = 0x00000040,
    /// <summary>Административные поля (aflds).</summary>
    AdminFields = 0x00000080,
    /// <summary>Расширенные свойства (hw, uid, ph, ph2, psw).</summary>
    ExtendedProperties = 0x00000100,
    /// <summary>Доступные команды (cmds).</summary>
    Commands = 0x00000200,
    /// <summary>Последнее сообщение и местоположение (lmsg, pos).</summary>
    LastMessage = 0x00000400,
    /// <summary>Датчики (sens).</summary>
    Sensors = 0x00001000,
    /// <summary>Счётчики (cnts).</summary>
    Counters = 0x00002000,
    /// <summary>Техобслуживание (mnt).</summary>
    Maintenance = 0x00010000,
    /// <summary>Детектор поездок и расход топлива.</summary>
    TripDetector = 0x00040000,
    /// <summary>Параметры сообщений.</summary>
    MessageParameters = 0x00100000,
    /// <summary>Подключение.</summary>
    Connection = 0x00200000,
    /// <summary>Местоположение.</summary>
    Location = 0x00400000,
    /// <summary>Задачи.</summary>
    Tasks = 0x04000000,
    /// <summary>Все возможные флаги.</summary>
    All = 0x3FFFFFFFFFFFFFFF,
}
