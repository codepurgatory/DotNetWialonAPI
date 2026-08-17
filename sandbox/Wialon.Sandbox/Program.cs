using DotNetEnv;
using Wialon.Sdk;
using Wialon.Sandbox.Demos;

// Загрузка .env файла
Env.Load(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", ".env"));

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "Wialon API SDK — Sandbox";

PrintBanner();

var client = WialonClient.FromEnvironment();

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("🔐 Подключение к Wialon API...");
Console.ResetColor();

try
{
    var session = await client.LoginAsync();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"✅ Авторизован как: {session.User?.Name} (ID: {session.User?.Id})");
    Console.WriteLine($"   Сессия: {session.Eid[..8]}...");
    Console.ResetColor();
    Console.WriteLine();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ Ошибка авторизации: {ex.Message}");
    Console.ResetColor();
    Console.WriteLine("\nПроверьте WIALON_ACCESS_TOKEN в файле .env");
    return;
}

// Интерактивное меню
while (true)
{
    PrintMenu();
    var key = Console.ReadKey(intercept: true).Key;
    Console.WriteLine();

    try
    {
        switch (key)
        {
            case ConsoleKey.D1: await AuthDemo.RunAsync(client); break;
            case ConsoleKey.D2: await SearchDemo.RunAsync(client); break;
            case ConsoleKey.D3: await MessagesDemo.RunAsync(client); break;
            case ConsoleKey.D4: await UnitsDemo.RunAsync(client); break;
            case ConsoleKey.D5: await GeoZonesDemo.RunAsync(client); break;
            case ConsoleKey.D6: await ReportsDemo.RunAsync(client); break;
            case ConsoleKey.D7: await BatchDemo.RunAsync(client); break;
            case ConsoleKey.Q:
            case ConsoleKey.Escape:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("👋 Выход... Завершение сессии.");
                Console.ResetColor();
                await client.LogoutAsync();
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Неизвестная команда.");
                Console.ResetColor();
                break;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Ошибка: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
    Console.ReadKey(intercept: true);
    Console.Clear();
}

static void PrintBanner()
{
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("""
    ╔══════════════════════════════════════════════════╗
    ║        🛰  Wialon API SDK  —  Sandbox            ║
    ║        C# .NET 8.0 | Wialon Remote API           ║
    ╚══════════════════════════════════════════════════╝
    """);
    Console.ResetColor();
}

static void PrintMenu()
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("╔═══════════════ Меню ═══════════════╗");
    Console.WriteLine("║  1 — Информация о сессии            ║");
    Console.WriteLine("║  2 — Поиск объектов и пользователей ║");
    Console.WriteLine("║  3 — Последние сообщения объекта    ║");
    Console.WriteLine("║  4 — Местоположение объектов        ║");
    Console.WriteLine("║  5 — Геозоны ресурса                ║");
    Console.WriteLine("║  6 — Запуск отчёта                  ║");
    Console.WriteLine("║  7 — Пакетный запрос (batch)        ║");
    Console.WriteLine("║  Q — Выход                          ║");
    Console.WriteLine("╚════════════════════════════════════╝");
    Console.Write("Выбор: ");
    Console.ResetColor();
}
