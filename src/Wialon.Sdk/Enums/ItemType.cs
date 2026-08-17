namespace Wialon.Sdk.Enums;

/// <summary>
/// Типы элементов Wialon.
/// Используется в core/search_items параметр itemsType.
/// </summary>
public enum ItemType
{
    /// <summary>Объект (транспортное средство / устройство).</summary>
    AvlUnit,
    /// <summary>Группа объектов.</summary>
    AvlUnitGroup,
    /// <summary>Ресурс / учётная запись (содержит геозоны, водителей, уведомления, отчёты).</summary>
    AvlResource,
    /// <summary>Пользователь.</summary>
    User,
    /// <summary>Ретранслятор.</summary>
    AvlRetranslator,
    /// <summary>Маршрут.</summary>
    AvlRoute
}

public static class ItemTypeExtensions
{
    public static string ToApiString(this ItemType type) => type switch
    {
        ItemType.AvlUnit        => "avl_unit",
        ItemType.AvlUnitGroup   => "avl_unit_group",
        ItemType.AvlResource    => "avl_resource",
        ItemType.User           => "user",
        ItemType.AvlRetranslator=> "avl_retranslator",
        ItemType.AvlRoute       => "avl_route",
        _                       => throw new ArgumentOutOfRangeException(nameof(type))
    };
}
