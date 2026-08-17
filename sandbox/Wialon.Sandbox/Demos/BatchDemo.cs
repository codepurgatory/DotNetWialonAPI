using Wialon.Sdk;
using Wialon.Sdk.Services;

namespace Wialon.Sandbox.Demos;

public static class BatchDemo
{
    public static async Task RunAsync(WialonClient client)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== [7] Пакетные запросы (core/batch) ===");
        Console.ResetColor();

        Console.WriteLine("Отправка 3 команд одним HTTP-запросом...");

        var requests = new[]
        {
            new BatchRequest
            {
                Svc = "core/check_unique",
                Params = new { type = "user", value = "admin" }
            },
            new BatchRequest
            {
                Svc = "core/check_unique",
                Params = new { type = "user", value = "non_existing_user_999999" }
            },
            new BatchRequest
            {
                Svc = "token/list",
                Params = new { }
            }
        };

        var responses = await client.Core.BatchAsync(requests);

        Console.WriteLine($"\nПолучено ответов: {responses.Count}");
        for (int i = 0; i < responses.Count; i++)
        {
            Console.WriteLine($"Ответ [{i}]: {responses[i].GetRawText()}");
        }
    }
}
