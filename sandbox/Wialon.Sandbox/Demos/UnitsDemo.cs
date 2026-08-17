using Wialon.Sdk;

namespace Wialon.Sandbox.Demos;

public static class UnitsDemo
{
    public static async Task RunAsync(WialonClient client)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== [4] Местоположение и данные объекта ===");
        Console.ResetColor();

        Console.Write("Введите ID объекта: ");
        var input = Console.ReadLine();
        if (!long.TryParse(input, out var unitId))
        {
            Console.WriteLine("Некорректный ID.");
            return;
        }

        Console.WriteLine($"\n🚗 Получение данных объекта {unitId}...");
        // 1025 = 1 (Base) + 1024 (Last message and position)
        var unit = await client.Core.SearchUnitAsync(unitId, flags: 1025);
        if (unit == null)
        {
            Console.WriteLine("Объект не найден.");
            return;
        }

        Console.WriteLine($"Название:    {unit.Name}");
        Console.WriteLine($"ID:          {unit.Id}");
        Console.WriteLine($"UID:         {unit.UniqueId ?? "н/д"}");
        Console.WriteLine($"Телефон:     {unit.Phone ?? "н/д"}");

        if (unit.Position != null)
        {
            Console.WriteLine("\n📍 Последняя позиция:");
            Console.WriteLine($"  Широта (Lat):   {unit.Position.Latitude:F6}");
            Console.WriteLine($"  Долгота (Lon):  {unit.Position.Longitude:F6}");
            Console.WriteLine($"  Скорость:       {unit.Position.Speed} км/ч");
            Console.WriteLine($"  Курс:           {unit.Position.Course}°");
            Console.WriteLine($"  Высота:         {unit.Position.Altitude} м");
            Console.WriteLine($"  Спутников:      {unit.Position.Satellites}");
            if (unit.Position.Time.HasValue)
            {
                var dt = DateTimeOffset.FromUnixTimeSeconds(unit.Position.Time.Value).UtcDateTime;
                Console.WriteLine($"  Время фиксации: {dt:yyyy-MM-dd HH:mm:ss} UTC");
            }
        }
        else
        {
            Console.WriteLine("Позиция пока отсутствует.");
        }
    }
}
