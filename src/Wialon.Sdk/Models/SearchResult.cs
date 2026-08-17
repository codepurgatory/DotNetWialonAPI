using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wialon.Sdk.Models;

/// <summary>
/// Результат поиска элементов (core/search_items).
/// </summary>
public sealed class SearchResult<T>
{
    [JsonPropertyName("searchSpec")]
    public SearchSpec? SearchSpec { get; set; }

    [JsonPropertyName("dataFlags")]
    public long DataFlags { get; set; }

    [JsonPropertyName("totalItemsCount")]
    public int TotalItemsCount { get; set; }

    [JsonPropertyName("indexFrom")]
    public int IndexFrom { get; set; }

    [JsonPropertyName("indexTo")]
    public int IndexTo { get; set; }

    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();
}

/// <summary>
/// Спецификация поиска.
/// </summary>
public sealed class SearchSpec
{
    [JsonPropertyName("itemsType")]
    public string ItemsType { get; set; } = string.Empty;

    [JsonPropertyName("propName")]
    public string PropName { get; set; } = string.Empty;

    [JsonPropertyName("propValueMask")]
    public string PropValueMask { get; set; } = string.Empty;

    [JsonPropertyName("sortType")]
    public string SortType { get; set; } = string.Empty;

    [JsonPropertyName("propType")]
    public string? PropType { get; set; }

    [JsonPropertyName("or_logic")]
    public string? OrLogic { get; set; }
}

/// <summary>
/// Сессия Wialon (результат token/login).
/// </summary>
public sealed class Session
{
    /// <summary>ID сессии (sid/eid).</summary>
    [JsonPropertyName("eid")]
    public string Eid { get; set; } = string.Empty;

    /// <summary>Информация о пользователе.</summary>
    [JsonPropertyName("user")]
    public SessionUser? User { get; set; }

    /// <summary>Версия API.</summary>
    [JsonPropertyName("api_version")]
    public int? ApiVersion { get; set; }

    /// <summary>Хост сервера.</summary>
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    /// <summary>Включённые функции.</summary>
    [JsonPropertyName("features")]
    public JsonElement? Features { get; set; }

    public override string ToString() => $"Session[{Eid[..8]}...] user={User?.Name}";
}

/// <summary>
/// Базовая информация о пользователе в сессии.
/// </summary>
public sealed class SessionUser
{
    [JsonPropertyName("nm")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("cls")] public int ClassId { get; set; }
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("uacl")] public long UserAccessLevel { get; set; }
    [JsonPropertyName("bact")] public long? BillingAccountId { get; set; }
    [JsonPropertyName("prp")] public Dictionary<string, string>? CustomProperties { get; set; }

    public override string ToString() => $"User[{Id}] '{Name}'";
}

/// <summary>
/// Результат загрузки сообщений (messages/load_interval, messages/load_last).
/// </summary>
public sealed class MessagesResult
{
    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; } = new();

    [JsonPropertyName("totalTime")]
    public long? TotalTime { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }
}

/// <summary>
/// Результат выполнения отчёта.
/// </summary>
public sealed class ReportResult
{
    [JsonPropertyName("reportResult")]
    public ReportResultData? ReportData { get; set; }
}

public sealed class ReportResultData
{
    [JsonPropertyName("tables")]
    public List<ReportTable> Tables { get; set; } = new();

    [JsonPropertyName("stats")]
    public List<List<string>>? Stats { get; set; }
}

public sealed class ReportTable
{
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("label")] public string Label { get; set; } = string.Empty;
    [JsonPropertyName("total_rows")] public int TotalRows { get; set; }
    [JsonPropertyName("header")] public List<string>? Header { get; set; }
    [JsonPropertyName("rows")] public List<ReportRow>? Rows { get; set; }
}

public sealed class ReportRow
{
    [JsonPropertyName("n")] public int RowIndex { get; set; }
    [JsonPropertyName("i1")] public int? I1 { get; set; }
    [JsonPropertyName("i2")] public int? I2 { get; set; }
    [JsonPropertyName("c")] public List<JsonElement>? Cells { get; set; }
    [JsonPropertyName("t")] public string? TotalType { get; set; }
}
