namespace Wialon.Sdk.Extensions;

/// <summary>
/// Вспомогательные методы для работы с временем в Wialon API.
/// Wialon использует Unix-время (секунды с 01.01.1970 UTC).
/// </summary>
public static class TimeExtensions
{
    private static readonly DateTimeOffset UnixEpoch = DateTimeOffset.UnixEpoch;

    /// <summary>
    /// Конвертирует DateTime в Unix-время (секунды).
    /// </summary>
    public static long ToUnixTime(this DateTime dt)
        => new DateTimeOffset(dt.ToUniversalTime()).ToUnixTimeSeconds();

    /// <summary>
    /// Конвертирует DateTimeOffset в Unix-время (секунды).
    /// </summary>
    public static long ToUnixTime(this DateTimeOffset dt)
        => dt.ToUnixTimeSeconds();

    /// <summary>
    /// Конвертирует Unix-время (секунды) в DateTime UTC.
    /// </summary>
    public static DateTime FromUnixTime(this long unixSeconds)
        => DateTimeOffset.FromUnixTimeSeconds(unixSeconds).UtcDateTime;

    /// <summary>
    /// Конвертирует Unix-время (секунды) в DateTimeOffset.
    /// </summary>
    public static DateTimeOffset ToDateTimeOffset(this long unixSeconds)
        => DateTimeOffset.FromUnixTimeSeconds(unixSeconds);

    /// <summary>
    /// Кодирует часовой пояс и параметр DST в значение tz для Wialon API.
    /// Документация: https://sdk.wialon.com/wiki/en/sidebar/remoteapi/apiref/format/datetime
    /// </summary>
    /// <param name="utcOffsetSeconds">Смещение от UTC в секундах (например, +3600 для UTC+1, -3600 для UTC-1).</param>
    /// <param name="dstFlag">Флаг летнего времени (например, 0x08000000 — нет DST, 0x0B030000 — Европа).</param>
    /// <returns>Значение tz для Wialon API.</returns>
    /// <example>
    /// UTC-01:00 без DST: EncodeTz(-3600, 0x08000000) → -134155792
    /// </example>
    public static int EncodeTz(int utcOffsetSeconds, int dstFlag = 0x08000000)
    {
        // Применяем маску к смещению UTC и объединяем с флагом DST
        int masked = (int)(utcOffsetSeconds & 0xf000ffff);
        return masked | dstFlag;
    }

    /// <summary>
    /// Декодирует значение tz из Wialon API в смещение UTC (секунды).
    /// </summary>
    /// <param name="tz">Значение tz из Wialon API.</param>
    /// <returns>Смещение от UTC в секундах.</returns>
    /// <example>
    /// ParseTz(-134155792) → -3600 (UTC-01:00)
    /// </example>
    public static int ParseTz(int tz)
    {
        // Применяем маску 0xffff для получения нижних 16 бит
        int lower16 = tz & 0xffff;

        // Если значение было отрицательным — применяем OR с 0xffff0000
        if (lower16 > 0x7fff)
            return (int)(lower16 | 0xffff0000);

        return lower16;
    }

    /// <summary>
    /// Извлекает флаг DST из значения tz.
    /// </summary>
    public static int GetDstFlag(int tz) => (int)(tz & 0x0fff0000);

    /// <summary>
    /// Возвращает TimeSpan смещения UTC для значения tz.
    /// </summary>
    public static TimeSpan GetUtcOffset(int tz)
        => TimeSpan.FromSeconds(ParseTz(tz));
}
