using Wialon.Sdk;

namespace Wialon.Sandbox.Demos;

public static class MessagesDemo
{
    public static async Task RunAsync(WialonClient client)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== [3] Загрузка последних сообщений объекта ===");
        Console.ResetColor();

        Console.Write("Введите ID объекта: ");
        var input = Console.ReadLine();
        if (!long.TryParse(input, out var unitId))
        {
            Console.WriteLine("Некорректный ID.");
            return;
        }

        Console.Write("Количество сообщений (по умолчанию 5): ");
        if (!int.TryParse(Console.ReadLine(), out var count) || count <= 0) count = 5;

        Console.WriteLine($"\n📡 Запрос {count} последних сообщений для объекта {unitId}...");
        var result = await client.Messages.LoadLastAsync(unitId, lastCount: count);

        Console.WriteLine($"Получено сообщений: {result.Messages.Count}");
        foreach (var msg in result.Messages)
        {
            Console.WriteLine($" • [{msg.Type}] Время: {msg.DateTime:yyyy-MM-dd HH:mm:ss} UTC");
            if (msg.Position != null)
            {
                Console.WriteLine($"   Координаты: Lat={msg.Position.Latitude:F6}, Lon={msg.Position.Longitude:F6}, Скорость={msg.Position.Speed} км/ч, Высота={msg.Position.Altitude} м");
            }
            if (msg.Parameters != null && msg.Parameters.Count > 0)
            {
                Console.WriteLine($"   Параметры: {string.Join(", ", msg.Parameters.Select(p => $"{p.Key}={p.Value}"))}");
            }
        }
    }
}
