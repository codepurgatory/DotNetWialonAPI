using Wialon.Sdk;

namespace Wialon.Sandbox.Demos;

public static class ReportsDemo
{
    public static async Task RunAsync(WialonClient client)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== [6] Выполнение отчёта ===");
        Console.ResetColor();

        Console.Write("ID ресурса с отчётом: ");
        if (!long.TryParse(Console.ReadLine(), out var resId)) return;

        Console.Write("ID шаблона отчёта: ");
        if (!long.TryParse(Console.ReadLine(), out var templateId)) return;

        Console.Write("ID объекта: ");
        if (!long.TryParse(Console.ReadLine(), out var unitId)) return;

        var to = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var from = to - (24 * 3600); // за последние 24 часа

        Console.WriteLine($"\n📊 Запуск отчёта за 24 часа (от {DateTimeOffset.FromUnixTimeSeconds(from):g} до {DateTimeOffset.FromUnixTimeSeconds(to):g})...");

        var reportResult = await client.Reports.ExecReportAsync(
            resourceId: resId,
            reportTemplateId: templateId,
            objectId: unitId,
            objectSecondId: 0,
            intervalFrom: from,
            intervalTo: to);

        Console.WriteLine("Отчёт выполнен.");
        var tables = await client.Reports.GetReportTablesAsync();
        Console.WriteLine($"Получено таблиц: {tables.Count}");
        for (int i = 0; i < tables.Count; i++)
        {
            var t = tables[i];
            Console.WriteLine($"Таблица [{i}]: {t.Name} ({t.Label}), строк: {t.TotalRows}");

            if (t.TotalRows > 0)
            {
                var rows = await client.Reports.GetResultRowsAsync(i, 0, Math.Min(5, t.TotalRows));
                Console.WriteLine($"  Первые {rows.Count} строк загружены.");
            }
        }

        Console.WriteLine("🧹 Очистка результатов отчёта...");
        await client.Reports.CleanupResultAsync();
        Console.WriteLine("Готово.");
    }
}
