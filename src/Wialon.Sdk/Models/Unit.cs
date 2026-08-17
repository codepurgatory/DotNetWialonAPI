using System.Text.Json.Serialization;

namespace Wialon.Sdk.Models;

/// <summary>
/// Объект мониторинга Wialon (avl_unit) — транспортное средство, устройство.
/// </summary>
public sealed class Unit
{
    /// <summary>Название объекта.</summary>
    [JsonPropertyName("nm")]
    public string Name { get; set; } = string.Empty;

    /// <summary>ID класса (всегда 2 для avl_unit).</summary>
    [JsonPropertyName("cls")]
    public int ClassId { get; set; }

    /// <summary>Уникальный ID объекта.</summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>Права доступа текущего пользователя.</summary>
    [JsonPropertyName("uacl")]
    public long UserAccessLevel { get; set; }

    /// <summary>Последнее известное местоположение.</summary>
    [JsonPropertyName("pos")]
    public Position? Position { get; set; }

    /// <summary>Последнее сообщение.</summary>
    [JsonPropertyName("lmsg")]
    public Message? LastMessage { get; set; }

    /// <summary>Пользовательские свойства (prp).</summary>
    [JsonPropertyName("prp")]
    public Dictionary<string, string>? CustomProperties { get; set; }

    /// <summary>ID создателя.</summary>
    [JsonPropertyName("crt")]
    public long? CreatorId { get; set; }

    /// <summary>ID учётной записи биллинга.</summary>
    [JsonPropertyName("bact")]
    public long? BillingAccountId { get; set; }

    /// <summary>Система измерений (0=СИ, 1=американская, 2=имперская).</summary>
    [JsonPropertyName("mu")]
    public int? MeasurementUnit { get; set; }

    /// <summary>Уникальный ID устройства (IMEI/ID).</summary>
    [JsonPropertyName("uid")]
    public string? UniqueId { get; set; }

    /// <summary>Уникальный ID устройства 2.</summary>
    [JsonPropertyName("uid2")]
    public string? UniqueId2 { get; set; }

    /// <summary>ID типа оборудования.</summary>
    [JsonPropertyName("hw")]
    public long? HardwareTypeId { get; set; }

    /// <summary>Номер телефона.</summary>
    [JsonPropertyName("ph")]
    public string? Phone { get; set; }

    /// <summary>Номер телефона 2.</summary>
    [JsonPropertyName("ph2")]
    public string? Phone2 { get; set; }

    /// <summary>URI изображения.</summary>
    [JsonPropertyName("uri")]
    public string? ImageUri { get; set; }

    /// <summary>Счётчик изменений изображения.</summary>
    [JsonPropertyName("ugi")]
    public int? ImageCounter { get; set; }

    /// <summary>Произвольные поля (flds).</summary>
    [JsonPropertyName("flds")]
    public Dictionary<string, CustomField>? CustomFields { get; set; }

    /// <summary>Административные поля (aflds).</summary>
    [JsonPropertyName("aflds")]
    public Dictionary<string, CustomField>? AdminFields { get; set; }

    /// <summary>Датчики (sens).</summary>
    [JsonPropertyName("sens")]
    public Dictionary<string, Sensor>? Sensors { get; set; }

    /// <summary>Время создания объекта (Unix UTC).</summary>
    [JsonPropertyName("ct")]
    public long? CreatedAt { get; set; }

    /// <summary>Активен ли объект.</summary>
    [JsonPropertyName("act")]
    public int? Active { get; set; }

    public override string ToString() => $"Unit[{Id}] '{Name}'";
}
