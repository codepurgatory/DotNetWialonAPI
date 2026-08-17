using System.Globalization;
using Wialon.Sdk;

namespace Wialon.Sandbox.Demos;

public static class GeoZonesDemo
{
    public static async Task RunAsync(WialonClient client)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== [5] Работа с геозонами ===");
        Console.ResetColor();

        Console.Write("Введите ID ресурса (avl_resource): ");
        var input = Console.ReadLine();
        if (!long.TryParse(input, out var resId))
        {
            Console.WriteLine("Некорректный ID ресурса.");
            return;
        }

        Console.WriteLine($"\n🗺 Загрузка геозон для ресурса {resId}...");
        // Флаг 4096 (0x1000) - геозоны
        var resObj = await client.Core.SearchItemAsync(resId, flags: 0x1001);
        Console.WriteLine($"Ответ получен: {resObj.GetRawText()[..Math.Min(resObj.GetRawText().Length, 200)]}...");

        Console.WriteLine("\nПроверка геозон по координатам точки.");
        Console.Write("Широта (Lat): ");
        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out var lat)) return;

        Console.Write("Долгота (Lon): ");
        if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out var lon)) return;

        Console.WriteLine($"🔍 Поиск геозон содержащих ({lat}, {lon})...");
        var zoneIds = await client.Resources.GetZonesByPointAsync(resId, lat, lon);
        Console.WriteLine($"Найдено геозон: {zoneIds.Count}");
        foreach (var zid in zoneIds)
        {
            Console.WriteLine($" • GeoZone ID: {zid}");
        }
    }
}
