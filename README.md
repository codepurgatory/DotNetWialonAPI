**English** | [Русский](README.ru.md)

![HEADER](assets/header.png)
<h1 align="center">.Net Wialon API Library</h1>
<h3 align="center">Wialon Hosting • SDK • Remote API • DotNet Library</h3>

![Static Badge](https://img.shields.io/badge/Project_Status-Testing-512bd4?style=flat-square&logo=dotnet&logoColor=512bd4)

# Wialon API SDK for .NET (C#)

> [!IMPORTANT]
> ### ⚠️ WARNING: MANDATORY READING — [SECURITY.MD](SECURITY.md)!
> Before using this library, it is **strongly recommended to carefully review [SECURITY.md](SECURITY.md)**.
> This project is currently in the **active testing and validation stage**. Use of this library is **strictly at your own risk**.

A full-featured asynchronous C# (.NET 8.0) client library (SDK) for interacting with the Wialon Remote API (Wialon Hosting and Wialon Local).

---

## 🚀 Features

- **Full Wialon API Coverage**: 13 specialized services:
  - `Core` (`core/*`) — authentication, item/unit/user/resource search (`search_item`, `search_items`), batch requests (`batch`), object creation, uniqueness check.
  - `Items` (`item/*`) — property editing, renaming, custom and administrative fields, log retrieval.
  - `Units` (`unit/*`) — unit property management, calculation flags, commands/tasks.
  - `Users` (`user/*`) — user management, access rights, passwords, locale settings.
  - `Resources` (`resource/*`) — geofences, drivers, trailers, notifications, jobs, POIs.
  - `Messages` (`messages/*`) — telematics data export, SMS, commands, events by interval or last N messages.
  - `Reports` (`report/*`) — report execution, table structure and row extraction, report export.
  - `Events` (`events/*`) — real-time event subscriptions, updates polling (`check_updates`).
  - `Tokens` (`token/*`) — API token management, creation with access permissions, deletion.
  - `Retranslators` (`retranslator/*`) — retranslator management and statistics.
  - `Files` (`file/*`) — file storage management.
  - `Exchange` (`exchange/*`) — data and messages import/export in JSON format.
  - `Render` (`render/*`) — map layers and track rendering.
- **Strong Typing**: Models for `Unit`, `Position`, `Message`, `GeoZone`, `Driver`, `SearchResult<T>`, `Session`, and more.
- **Error Handling**: Automatic parsing of `{"error": <code>}` responses into strongly typed `WialonException` instances with the `WialonErrorCode` enum.
- **Automatic Retry**: Automatic retry mechanism on rate-limit errors (`1003`).
- **Interactive Sandbox**: `Wialon.Sandbox` console application for interactive testing and demonstration of all scenarios.
- **Unit Tests**: Test suite with mocked HTTP clients for offline testing without hitting live servers.

---

## 📦 Solution Structure

```
Wialon API SDK/
├── src/
│   └── Wialon.Sdk/         # Core class library (.NET 8)
├── tests/
│   └── Wialon.Sdk.Tests/   # xUnit unit test suite
├── sandbox/
│   └── Wialon.Sandbox/     # Interactive console sandbox
├── .env                    # Host URL and token (ignored by git)
├── .env.example            # Configuration example
├── SECURITY.md             # Security policy and disclaimer
└── Wialon.sln              # Solution file
```

---

## ⚙️ Configuration & Setup

Create or edit the `.env` file in the project root:

```env
# 72-character Wialon Access Token
WIALON_ACCESS_TOKEN=your_72_character_token_here

# Wialon Host (defaults to hst-api.wialon.com or your Wialon Local address)
WIALON_API_HOST=https://hst-api.wialon.com
```

### How to get an Access Token:
Open this URL in your browser (replace `<host>` with your Wialon server address):
```
https://<host>/login.html?client_id=MyApp&access_type=-1&duration=0
```

---

## 💻 Quick Start

### Initialization and Login

```csharp
using Wialon.Sdk;

// Option 1: Load from environment variables (.env)
var client = WialonClient.FromEnvironment();
var session = await client.LoginAsync();

// Option 2: Explicit options
var client = new WialonClient(new WialonClientOptions
{
    Host = "https://hst-api.wialon.com",
    AccessToken = "your_token"
});
await client.LoginAsync();
```

### Searching Units

```csharp
// Search all units by pattern
var searchResult = await client.Core.SearchUnitsAsync("*");
foreach (var unit in searchResult.Items)
{
    Console.WriteLine($"Unit: {unit.Name}, ID: {unit.Id}");
}

// Fetch a single unit with its last known position
var unitWithPos = await client.Core.SearchUnitAsync(12345, flags: 1025);
if (unitWithPos?.Position != null)
{
    Console.WriteLine($"Coordinates: {unitWithPos.Position.Latitude}, {unitWithPos.Position.Longitude}");
    Console.WriteLine($"Speed: {unitWithPos.Position.Speed} km/h");
}
```

### Loading Telematics Messages

```csharp
// Load last 10 messages
var messages = await client.Messages.LoadLastAsync(unitId: 12345, lastCount: 10);
foreach (var msg in messages.Messages)
{
    Console.WriteLine($"[{msg.DateTime}] Speed: {msg.Position?.Speed}, Fuel: {msg.Parameters?["fuel"]}");
}
```

### Batch Requests

```csharp
var results = await client.Core.BatchAsync(new[]
{
    new BatchRequest { Svc = "core/check_unique", Params = new { type = "user", value = "admin" } },
    new BatchRequest { Svc = "token/list", Params = new { } }
});
```

---

## 🧪 Running Tests

```bash
dotnet test
```

---

## 🎮 Running the Sandbox

```bash
dotnet run --project sandbox/Wialon.Sandbox
```

---

## 🔒 Security

Make sure to read [SECURITY.md](SECURITY.md) for security guidelines, token handling recommendations, and disclaimers.

> **Reminder:** Always thoroughly test your API integration workflows in an isolated sandbox or test environment (separate account, separate resource, isolated test units) before deploying to production systems to avoid data loss, corruption, or account suspension.