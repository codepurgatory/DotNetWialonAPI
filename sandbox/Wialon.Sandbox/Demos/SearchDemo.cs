using Wialon.Sdk;

namespace Wialon.Sandbox.Demos;

public static class SearchDemo
{
    public static async Task RunAsync(WialonClient client)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== [2] Поиск объектов и пользователей ===");
        Console.ResetColor();

        Console.Write("Введите маску имени объекта (по умолчанию '*'): ");
        var mask = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(mask)) mask = "*";

        Console.WriteLine($"\n🔍 Поиск объектов (avl_unit) с маской '{mask}'...");
        var unitsResult = await client.Core.SearchUnitsAsync(mask, flags: 0x441, from: 0, to: 10);
        Console.WriteLine($"Найдено объектов: {unitsResult.TotalItemsCount}");
        foreach (var unit in unitsResult.Items)
        {
            Console.WriteLine($" • [{unit.Id}] {unit.Name} (Class: {unit.ClassId})");
        }

        Console.WriteLine("\n🔍 Поиск пользователей (user)...");
        var usersResult = await client.Core.SearchUsersAsync("*", flags: 1, from: 0, to: 10);
        Console.WriteLine($"Найдено пользователей: {usersResult.TotalItemsCount}");
        foreach (var user in usersResult.Items)
        {
            Console.WriteLine($" • [{user.Id}] {user.Name}");
        }

        Console.WriteLine("\n🔍 Поиск ресурсов (avl_resource)...");
        var resResult = await client.Core.SearchResourcesAsync("*", flags: 1, from: 0, to: 10);
        Console.WriteLine($"Найдено ресурсов: {resResult.TotalItemsCount}");
        foreach (var res in resResult.Items)
        {
            Console.WriteLine($" • [{res.Id}] {res.Name}");
        }
    }
}
