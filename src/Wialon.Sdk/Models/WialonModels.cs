using System.Text.Json.Serialization;

namespace Wialon.Sdk.Models;

/// <summary>
/// Пользователь Wialon.
/// </summary>
public sealed class WialonUser
{
    [JsonPropertyName("nm")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("cls")] public int ClassId { get; set; }
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("uacl")] public long UserAccessLevel { get; set; }
    [JsonPropertyName("mu")] public int? MeasurementUnit { get; set; }
    [JsonPropertyName("prp")] public Dictionary<string, string>? CustomProperties { get; set; }
    [JsonPropertyName("flds")] public Dictionary<string, CustomField>? CustomFields { get; set; }
    [JsonPropertyName("aflds")] public Dictionary<string, CustomField>? AdminFields { get; set; }
    [JsonPropertyName("fl")] public long? Flags { get; set; }
    [JsonPropertyName("hm")] public string? HostMask { get; set; }
    [JsonPropertyName("ld")] public long? LastLogin { get; set; }
    [JsonPropertyName("crt")] public long? CreatorId { get; set; }
    [JsonPropertyName("bact")] public long? BillingAccountId { get; set; }
    [JsonPropertyName("gd")] public string? Guid { get; set; }

    public override string ToString() => $"User[{Id}] '{Name}'";
}

/// <summary>
/// Группа объектов Wialon (avl_unit_group).
/// </summary>
public sealed class UnitGroup
{
    [JsonPropertyName("nm")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("cls")] public int ClassId { get; set; }
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("uacl")] public long UserAccessLevel { get; set; }
    [JsonPropertyName("u")] public List<long>? UnitIds { get; set; }
    [JsonPropertyName("prp")] public Dictionary<string, string>? CustomProperties { get; set; }
    [JsonPropertyName("flds")] public Dictionary<string, CustomField>? CustomFields { get; set; }
    [JsonPropertyName("gd")] public string? Guid { get; set; }

    public override string ToString() => $"UnitGroup[{Id}] '{Name}' ({UnitIds?.Count ?? 0} units)";
}

/// <summary>
/// Ресурс / учётная запись Wialon (avl_resource).
/// </summary>
public sealed class Resource
{
    [JsonPropertyName("nm")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("cls")] public int ClassId { get; set; }
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("uacl")] public long UserAccessLevel { get; set; }
    [JsonPropertyName("prp")] public Dictionary<string, string>? CustomProperties { get; set; }
    [JsonPropertyName("flds")] public Dictionary<string, CustomField>? CustomFields { get; set; }
    [JsonPropertyName("aflds")] public Dictionary<string, CustomField>? AdminFields { get; set; }
    [JsonPropertyName("gd")] public string? Guid { get; set; }
    [JsonPropertyName("crt")] public long? CreatorId { get; set; }
    [JsonPropertyName("bact")] public long? BillingAccountId { get; set; }
    [JsonPropertyName("zl")] public Dictionary<string, GeoZone>? GeoZones { get; set; }
    [JsonPropertyName("zlmax")] public long? GeoZonesMax { get; set; }

    public override string ToString() => $"Resource[{Id}] '{Name}'";
}

/// <summary>
/// Геозона Wialon.
/// </summary>
public sealed class GeoZone
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("n")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("d")] public string Description { get; set; } = string.Empty;
    [JsonPropertyName("f")] public long Flags { get; set; }
    /// <summary>Тип: 1=линия, 2=полигон, 3=круг.</summary>
    [JsonPropertyName("t")] public int Type { get; set; }
    [JsonPropertyName("e")] public int Checksum { get; set; }
    [JsonPropertyName("b")] public GeoZoneBounds? Bounds { get; set; }

    public string TypeName => Type switch { 1 => "Line", 2 => "Polygon", 3 => "Circle", _ => "Unknown" };
    public override string ToString() => $"GeoZone[{Id}] '{Name}' ({TypeName})";
}

/// <summary>Границы геозоны.</summary>
public sealed class GeoZoneBounds
{
    [JsonPropertyName("min_x")] public double MinLon { get; set; }
    [JsonPropertyName("min_y")] public double MinLat { get; set; }
    [JsonPropertyName("max_x")] public double MaxLon { get; set; }
    [JsonPropertyName("max_y")] public double MaxLat { get; set; }
    [JsonPropertyName("cen_x")] public double CenterLon { get; set; }
    [JsonPropertyName("cen_y")] public double CenterLat { get; set; }
}

/// <summary>Водитель.</summary>
public sealed class Driver
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("n")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("c")] public string Code { get; set; } = string.Empty;
    [JsonPropertyName("ds")] public string Description { get; set; } = string.Empty;
    [JsonPropertyName("p")] public string Phone { get; set; } = string.Empty;
    [JsonPropertyName("bu")] public long? BoundUnitId { get; set; }
    [JsonPropertyName("bt")] public long? BoundAt { get; set; }
    public override string ToString() => $"Driver[{Id}] '{Name}'";
}

/// <summary>Прицеп.</summary>
public sealed class Trailer
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("n")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("c")] public string Code { get; set; } = string.Empty;
    [JsonPropertyName("ds")] public string Description { get; set; } = string.Empty;
    [JsonPropertyName("bu")] public long? BoundUnitId { get; set; }
    [JsonPropertyName("bt")] public long? BoundAt { get; set; }
    public override string ToString() => $"Trailer[{Id}] '{Name}'";
}

/// <summary>Уведомление ресурса.</summary>
public sealed class Notification
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("n")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("ta")] public long ActivationTime { get; set; }
    [JsonPropertyName("td")] public long DeactivationTime { get; set; }
    [JsonPropertyName("ma")] public long MaxActivations { get; set; }
    [JsonPropertyName("fl")] public long NotificationTypeFlags { get; set; }
    [JsonPropertyName("ac")] public long ActivationsCount { get; set; }
    [JsonPropertyName("un")] public List<long>? UnitIds { get; set; }
    [JsonPropertyName("trg")] public string? TriggerType { get; set; }
    public override string ToString() => $"Notification[{Id}] '{Name}'";
}

/// <summary>POI (точка интереса).</summary>
public sealed class Poi
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("n")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("y")] public double Latitude { get; set; }
    [JsonPropertyName("x")] public double Longitude { get; set; }
    public override string ToString() => $"POI[{Id}] '{Name}' ({Latitude},{Longitude})";
}

/// <summary>Задание ресурса.</summary>
public sealed class Job
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("n")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("ta")] public long ActivationTime { get; set; }
    [JsonPropertyName("td")] public long DeactivationTime { get; set; }
    [JsonPropertyName("fl")] public long Flags { get; set; }
    [JsonPropertyName("un")] public List<long>? UnitIds { get; set; }
    public override string ToString() => $"Job[{Id}] '{Name}'";
}

/// <summary>Шаблон отчёта.</summary>
public sealed class ReportTemplate
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("n")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("ct")] public string Type { get; set; } = string.Empty;
    [JsonPropertyName("c")] public int Checksum { get; set; }
    public override string ToString() => $"ReportTemplate[{Id}] '{Name}' ({Type})";
}

/// <summary>Токен Wialon.</summary>
public sealed class WialonToken
{
    [JsonPropertyName("h")] public string Hash { get; set; } = string.Empty;
    [JsonPropertyName("app")] public string AppName { get; set; } = string.Empty;
    [JsonPropertyName("at")] public long ActivationTime { get; set; }
    [JsonPropertyName("ct")] public long CreationTime { get; set; }
    [JsonPropertyName("dur")] public long Duration { get; set; }
    [JsonPropertyName("fl")] public long AccessFlags { get; set; }
    [JsonPropertyName("items")] public List<long>? Items { get; set; }
    [JsonPropertyName("p")] public string? CustomParams { get; set; }
    public override string ToString() => $"Token '{AppName}' ({Hash[..8]}...)";
}

/// <summary>Ретранслятор.</summary>
public sealed class Retranslator
{
    [JsonPropertyName("nm")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("cls")] public int ClassId { get; set; }
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("uacl")] public long UserAccessLevel { get; set; }
    [JsonPropertyName("prp")] public Dictionary<string, string>? CustomProperties { get; set; }
    [JsonPropertyName("rtro")] public int? State { get; set; }
    public bool IsRunning => State == 1;
    public override string ToString() => $"Retranslator[{Id}] '{Name}' ({(IsRunning ? "running" : "stopped")})";
}
