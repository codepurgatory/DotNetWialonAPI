using System.Text.Json;
using Wialon.Sdk.Extensions;
using Wialon.Sdk.Models;

namespace Wialon.Sdk.Tests.Models;

public sealed class UnitDeserializationTests
{
    [Fact]
    public void Unit_DeserializesFromSearchItemResponse()
    {
        // Arrange
        var json = """
        {
            "nm": "Bavarian Tractor",
            "cls": 2,
            "id": 34868,
            "uacl": 638138188323,
            "pos": {
                "y": 53.9205504,
                "x": 27.4921152,
                "z": 238,
                "s": 0,
                "c": 102,
                "sc": 10,
                "t": 1358761631
            },
            "lmsg": {
                "t": 1358761631,
                "f": 3,
                "tp": "ud",
                "p": {"pwr_ext": 12.356}
            }
        }
        """;

        // Act
        var unit = JsonSerializer.Deserialize<Unit>(json);

        // Assert
        Assert.NotNull(unit);
        Assert.Equal("Bavarian Tractor", unit.Name);
        Assert.Equal(34868, unit.Id);
        Assert.Equal(2, unit.ClassId);
        Assert.Equal(638138188323, unit.UserAccessLevel);
        Assert.NotNull(unit.Position);
        Assert.Equal(53.9205504, unit.Position!.Latitude, 5);
        Assert.Equal(27.4921152, unit.Position.Longitude, 5);
        Assert.Equal(238, unit.Position.Altitude);
        Assert.Equal(10, unit.Position.Satellites);
        Assert.NotNull(unit.LastMessage);
        Assert.Equal("ud", unit.LastMessage!.Type);
    }

    [Fact]
    public void Unit_ToStringContainsIdAndName()
    {
        var unit = new Unit { Id = 123, Name = "Test Unit" };
        Assert.Contains("123", unit.ToString());
        Assert.Contains("Test Unit", unit.ToString());
    }

    [Fact]
    public void Unit_WithCustomFields_DeserializesCorrectly()
    {
        var json = """
        {
            "nm": "Test",
            "cls": 2,
            "id": 1,
            "uacl": 1,
            "flds": {
                "1": {"id": 1, "n": "Type", "v": "Truck"},
                "2": {"id": 2, "n": "Region", "v": "Moscow"}
            }
        }
        """;

        var unit = JsonSerializer.Deserialize<Unit>(json);
        Assert.NotNull(unit?.CustomFields);
        Assert.Equal(2, unit!.CustomFields!.Count);
        Assert.Equal("Type", unit.CustomFields["1"].Name);
        Assert.Equal("Truck", unit.CustomFields["1"].Value);
    }
}

public sealed class MessageDeserializationTests
{
    [Fact]
    public void Message_DataType_DeserializesCorrectly()
    {
        var json = """
        {
            "t": 1614336759,
            "f": 3,
            "tp": "ud",
            "pos": {"y": 43.24, "x": -118.14, "z": 300, "s": 12, "c": 0, "sc": 8},
            "i": 0,
            "o": 0,
            "rt": 1614336760,
            "p": {"fuel": 220.79}
        }
        """;

        var msg = JsonSerializer.Deserialize<Message>(json);
        Assert.NotNull(msg);
        Assert.Equal("ud", msg.Type);
        Assert.Equal(1614336759, msg.Time);
        Assert.NotNull(msg.Position);
        Assert.Equal(43.24, msg.Position!.Latitude, 2);
        Assert.Equal(12, msg.Position.Speed);
        Assert.NotNull(msg.Parameters);
        Assert.True(msg.Parameters!.ContainsKey("fuel"));
    }

    [Fact]
    public void Message_SmsType_DeserializesCorrectly()
    {
        var json = """
        {
            "t": 1614336800,
            "f": 256,
            "tp": "us",
            "st": "Test SMS",
            "mp": "+79001234567",
            "p": {}
        }
        """;

        var msg = JsonSerializer.Deserialize<Message>(json);
        Assert.NotNull(msg);
        Assert.Equal("us", msg.Type);
        Assert.Equal("Test SMS", msg.SmsText);
        Assert.Equal("+79001234567", msg.ModemPhone);
    }

    [Fact]
    public void Message_DateTime_ConvertsFromUnixTime()
    {
        var msg = new Message { Time = 1614336759 };
        var expected = DateTimeOffset.FromUnixTimeSeconds(1614336759).UtcDateTime;
        Assert.Equal(expected, msg.DateTime);
    }
}

public sealed class PositionTests
{
    [Fact]
    public void Position_DeserializesFromJson()
    {
        var json = """{"y": 55.75, "x": 37.62, "z": 150, "s": 60, "c": 180, "sc": 12}""";
        var pos = JsonSerializer.Deserialize<Position>(json);
        Assert.NotNull(pos);
        Assert.Equal(55.75, pos.Latitude, 2);
        Assert.Equal(37.62, pos.Longitude, 2);
        Assert.Equal(150, pos.Altitude);
        Assert.Equal(60, pos.Speed);
        Assert.Equal(180, pos.Course);
        Assert.Equal(12, pos.Satellites);
    }

    [Fact]
    public void Position_ToString_ContainsCoordinates()
    {
        var pos = new Position { Latitude = 55.75, Longitude = 37.62, Speed = 60 };
        var str = pos.ToString();
        Assert.Contains("55.75", str);
        Assert.Contains("37.62", str);
        Assert.Contains("60", str);
    }
}

public sealed class SearchResultTests
{
    [Fact]
    public void SearchResult_DeserializesFromJson()
    {
        var json = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "search_items.json"));
        var result = JsonSerializer.Deserialize<SearchResult<Unit>>(json);
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalItemsCount);
        Assert.Equal(2, result.Items.Count);
        Assert.NotNull(result.SearchSpec);
        Assert.Equal("avl_unit", result.SearchSpec!.ItemsType);
    }
}

public sealed class FlagExtensionsTests
{
    [Fact]
    public void CombineFlags_MultipleFlagsOrTogether()
    {
        var combined = FlagExtensions.CombineFlags(0x1, 0x2, 0x4);
        Assert.Equal(0x7, combined);
    }

    [Fact]
    public void HasWialonFlag_FlagSet_ReturnsTrue()
    {
        long value = 0b1011;
        Assert.True(value.HasWialonFlag(0b0001));
        Assert.True(value.HasWialonFlag(0b0010));
        Assert.True(value.HasWialonFlag(0b1000));
    }

    [Fact]
    public void HasWialonFlag_FlagNotSet_ReturnsFalse()
    {
        long value = 0b1011;
        Assert.False(value.HasWialonFlag(0b0100));
    }

    [Fact]
    public void SetFlag_AddsFlag()
    {
        long value = 0b0001;
        var result = value.SetFlag(0b0010);
        Assert.Equal(0b0011, result);
    }

    [Fact]
    public void ClearFlag_RemovesFlag()
    {
        long value = 0b0111;
        var result = value.ClearFlag(0b0010);
        Assert.Equal(0b0101, result);
    }
}

public sealed class TimeExtensionsTests
{
    [Fact]
    public void ToUnixTime_FromDateTime_CorrectValue()
    {
        var dt = new DateTime(2021, 2, 26, 12, 32, 39, DateTimeKind.Utc);
        var unix = dt.ToUnixTime();
        Assert.Equal(1614342759, unix);
    }

    [Fact]
    public void FromUnixTime_CorrectDateTime()
    {
        long unix = 1614336759;
        var dt = unix.FromUnixTime();
        Assert.Equal(2021, dt.Year);
        Assert.Equal(2, dt.Month);
    }

    [Fact]
    public void EncodeTz_AzoresWith_NoDs_CorrectValue()
    {
        // Пример из документации: UTC-01:00 без DST → -134155792
        // -3600 & 0xf000ffff | 0x08000000
        var tz = TimeExtensions.EncodeTz(-3600, 0x08000000);
        Assert.Equal(-134155792, tz);
    }

    [Fact]
    public void ParseTz_Azores_CorrectOffset()
    {
        // Обратная операция: -134155792 → -3600
        var offset = TimeExtensions.ParseTz(-134155792);
        Assert.Equal(-3600, offset);
    }

    [Fact]
    public void GetUtcOffset_ReturnsCorrectTimespan()
    {
        var offset = TimeExtensions.GetUtcOffset(-134155792);
        Assert.Equal(TimeSpan.FromSeconds(-3600), offset);
    }

    [Fact]
    public void EncodeTz_Moscow_CorrectValue()
    {
        // UTC+3 (10800 секунд) без DST
        var tz = TimeExtensions.EncodeTz(10800, 0x08000000);
        Assert.Equal(10800 | 0x08000000, tz);
    }
}
