using System.Text.Json.Serialization;

namespace Wialon.Sdk.Models;

/// <summary>
/// Произвольное/административное поле элемента Wialon.
/// </summary>
public sealed class CustomField
{
    /// <summary>ID поля.</summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>Название поля.</summary>
    [JsonPropertyName("n")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Значение поля.</summary>
    [JsonPropertyName("v")]
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// Датчик объекта.
/// </summary>
public sealed class Sensor
{
    /// <summary>ID датчика.</summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>Имя датчика.</summary>
    [JsonPropertyName("n")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Тип датчика.</summary>
    [JsonPropertyName("t")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Параметр сообщения (имя).</summary>
    [JsonPropertyName("p")]
    public string Parameter { get; set; } = string.Empty;

    /// <summary>Описание.</summary>
    [JsonPropertyName("d")]
    public string Description { get; set; } = string.Empty;

    /// <summary>Метрика (единица измерения).</summary>
    [JsonPropertyName("m")]
    public string Metric { get; set; } = string.Empty;
}
