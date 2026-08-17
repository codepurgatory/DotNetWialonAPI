namespace Wialon.Sdk.Extensions;

/// <summary>
/// Вспомогательные методы для работы с флагами Wialon API.
/// В Wialon все флаги передаются в десятичном формате (DEC).
/// </summary>
public static class FlagExtensions
{
    /// <summary>
    /// Объединяет несколько флагов в одно числовое значение (сумма для Wialon API).
    /// </summary>
    public static long CombineFlags(params long[] flags)
    {
        long result = 0;
        foreach (var flag in flags)
            result |= flag;
        return result;
    }

    /// <summary>
    /// Проверяет, установлен ли указанный флаг в значении.
    /// </summary>
    public static bool HasWialonFlag(this long value, long flag) => (value & flag) == flag;

    /// <summary>
    /// Проверяет, установлен ли указанный флаг в значении (int версия).
    /// </summary>
    public static bool HasWialonFlag(this int value, int flag) => (value & flag) == flag;

    /// <summary>
    /// Конвертирует enum-флаг в long для передачи в API.
    /// </summary>
    public static long ToDecimal<T>(this T flag) where T : Enum
        => Convert.ToInt64(flag);

    /// <summary>
    /// Устанавливает флаг в значении.
    /// </summary>
    public static long SetFlag(this long value, long flag) => value | flag;

    /// <summary>
    /// Снимает флаг с значения.
    /// </summary>
    public static long ClearFlag(this long value, long flag) => value & ~flag;
}
