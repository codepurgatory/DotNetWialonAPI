using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wialon.Sdk.Models;

/// <summary>
/// Сообщение Wialon (данные телематики, SMS, команда, событие и т.д.).
/// </summary>
public sealed class Message
{
    /// <summary>Время сообщения (Unix UTC).</summary>
    [JsonPropertyName("t")]
    public long Time { get; set; }

    /// <summary>Флаги сообщения.</summary>
    [JsonPropertyName("f")]
    public long Flags { get; set; }

    /// <summary>Тип сообщения (ud=данные, us=SMS, ucr=команда, evt=событие, xx=уведомление).</summary>
    [JsonPropertyName("tp")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Местоположение (для сообщений с данными).</summary>
    [JsonPropertyName("pos")]
    public Position? Position { get; set; }

    /// <summary>Входные данные (битовая маска).</summary>
    [JsonPropertyName("i")]
    public long? Inputs { get; set; }

    /// <summary>Выходные данные (битовая маска).</summary>
    [JsonPropertyName("o")]
    public long? Outputs { get; set; }

    /// <summary>Параметры сообщения (ключ: значение).</summary>
    [JsonPropertyName("p")]
    public Dictionary<string, JsonElement>? Parameters { get; set; }

    /// <summary>Время регистрации сообщения на сервере (Unix UTC).</summary>
    [JsonPropertyName("rt")]
    public long? RegistrationTime { get; set; }

    /// <summary>Контрольная сумма LBS.</summary>
    [JsonPropertyName("lc")]
    public int? LbsChecksum { get; set; }

    // --- SMS-специфичные поля ---
    /// <summary>Текст SMS-сообщения.</summary>
    [JsonPropertyName("st")]
    public string? SmsText { get; set; }

    /// <summary>Номер телефона модема.</summary>
    [JsonPropertyName("mp")]
    public string? ModemPhone { get; set; }

    // --- Команда-специфичные поля ---
    /// <summary>Название команды.</summary>
    [JsonPropertyName("ca")]
    public string? CommandName { get; set; }

    /// <summary>Тип команды.</summary>
    [JsonPropertyName("cn")]
    public string? CommandType { get; set; }

    /// <summary>Параметры команды.</summary>
    [JsonPropertyName("cp")]
    public string? CommandParams { get; set; }

    /// <summary>ID пользователя, выполнившего команду.</summary>
    [JsonPropertyName("ui")]
    public long? UserId { get; set; }

    // --- Событие-специфичные поля ---
    /// <summary>Текст события.</summary>
    [JsonPropertyName("et")]
    public string? EventText { get; set; }

    /// <summary>Долгота события.</summary>
    [JsonPropertyName("x")]
    public double? EventLon { get; set; }

    /// <summary>Широта события.</summary>
    [JsonPropertyName("y")]
    public double? EventLat { get; set; }

    /// <summary>Конвертирует Unix-время в DateTime UTC.</summary>
    public DateTime DateTime => DateTimeOffset.FromUnixTimeSeconds(Time).UtcDateTime;

    public override string ToString()
        => $"[{Type}] {DateTime:yyyy-MM-dd HH:mm:ss} UTC | pos={Position}";
}
