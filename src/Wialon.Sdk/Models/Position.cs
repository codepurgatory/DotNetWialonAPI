using System.Text.Json.Serialization;

namespace Wialon.Sdk.Models;

/// <summary>
/// Местоположение объекта (координаты из Wialon API).
/// </summary>
public sealed class Position
{
    /// <summary>Широта (latitude).</summary>
    [JsonPropertyName("y")]
    public double Latitude { get; set; }

    /// <summary>Долгота (longitude).</summary>
    [JsonPropertyName("x")]
    public double Longitude { get; set; }

    /// <summary>Высота (altitude), метры.</summary>
    [JsonPropertyName("z")]
    public int Altitude { get; set; }

    /// <summary>Скорость, км/ч.</summary>
    [JsonPropertyName("s")]
    public int Speed { get; set; }

    /// <summary>Курс (направление движения), градусы 0-360.</summary>
    [JsonPropertyName("c")]
    public int Course { get; set; }

    /// <summary>Количество спутников.</summary>
    [JsonPropertyName("sc")]
    public int Satellites { get; set; }

    /// <summary>Время последнего обновления позиции (Unix-время UTC).</summary>
    [JsonPropertyName("t")]
    public long? Time { get; set; }

    /// <summary>Флаги сообщения.</summary>
    [JsonPropertyName("f")]
    public long? Flags { get; set; }

    /// <summary>Контрольная сумма LBS-сообщения.</summary>
    [JsonPropertyName("lc")]
    public int? LbsChecksum { get; set; }

    public override string ToString()
        => string.Create(System.Globalization.CultureInfo.InvariantCulture, $"({Latitude:F6}, {Longitude:F6}) speed={Speed} km/h alt={Altitude}m course={Course}°");
}
