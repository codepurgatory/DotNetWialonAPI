using Wialon.Sdk;

namespace Wialon.Sandbox.Demos;

public static class AuthDemo
{
    public static Task RunAsync(WialonClient client)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== [1] Информация о текущей сессии ===");
        Console.ResetColor();

        var session = client.CurrentSession;
        if (session == null)
        {
            Console.WriteLine("Сессия не активна.");
            return Task.CompletedTask;
        }

        Console.WriteLine($"Session ID (EID): {session.Eid}");
        Console.WriteLine($"API Version:      {session.ApiVersion}");
        Console.WriteLine($"Host:             {session.Host}");
        if (session.User != null)
        {
            Console.WriteLine($"User Name:        {session.User.Name}");
            Console.WriteLine($"User ID:          {session.User.Id}");
            Console.WriteLine($"Class ID:         {session.User.ClassId}");
            Console.WriteLine($"Billing Account:  {session.User.BillingAccountId}");
        }

        return Task.CompletedTask;
    }
}
