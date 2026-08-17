using Wialon.Sdk.Services;
using Wialon.Sdk.Tests.Fixtures;

namespace Wialon.Sdk.Tests.Services;

public sealed class CoreServiceTests
{
    private static async Task<WialonClient> GetLoggedInClientAsync(MockHttpHandler handler, string? searchResponse = null)
    {
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (_, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        if (searchResponse is not null)
            handler.Respond(searchResponse);
        await client.LoginAsync();
        return client;
    }

    [Fact]
    public async Task SearchUnitsAsync_ReturnsUnits()
    {
        // Arrange
        var searchJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "search_items.json"));
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond(searchJson);
        await client.LoginAsync();

        // Act
        var result = await client.Core.SearchUnitsAsync("*");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalItemsCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("Bavarian Tractor", result.Items[0].Name);
        Assert.Equal(34868, result.Items[0].Id);
        Assert.Equal("Volvo awesome", result.Items[1].Name);
    }

    [Fact]
    public async Task SearchUnitAsync_ById_ReturnsUnit()
    {
        // Arrange
        var searchJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "search_item.json"));
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond(searchJson);
        await client.LoginAsync();

        // Act
        var unit = await client.Core.SearchUnitAsync(34868);

        // Assert
        Assert.NotNull(unit);
        Assert.Equal(34868, unit.Id);
        Assert.Equal("Bavarian Tractor", unit.Name);
        Assert.NotNull(unit.Position);
        Assert.Equal(53.9205504, unit.Position!.Latitude, 5);
        Assert.Equal(27.4921152, unit.Position.Longitude, 5);
    }

    [Fact]
    public async Task SearchUnitsAsync_SendsCorrectRequest()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var searchJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "search_items.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond(searchJson);
        await client.LoginAsync();

        // Act
        await client.Core.SearchUnitsAsync("*Test*");

        // Assert
        var request = handler.LastRequest;
        Assert.NotNull(request);
        var rawBody = await request!.Content!.ReadAsStringAsync();
        var body = Uri.UnescapeDataString(rawBody);
        Assert.Contains("core/search_items", body);
        Assert.Contains("avl_unit", body);
        Assert.Contains("*Test*", body);
    }

    [Fact]
    public async Task CheckUniqueAsync_UniqueItem_ReturnsTrue()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond("""{"result":0}""");
        await client.LoginAsync();

        // Act
        var isUnique = await client.Core.CheckUniqueAsync("user", "new_user_name");

        // Assert
        Assert.True(isUnique);
    }

    [Fact]
    public async Task CheckUniqueAsync_DuplicateItem_ReturnsFalse()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond("""{"result":1}""");
        await client.LoginAsync();

        // Act
        var isUnique = await client.Core.CheckUniqueAsync("user", "existing_user");

        // Assert
        Assert.False(isUnique);
    }

    [Fact]
    public async Task BatchAsync_MultipleCalls_ReturnsResults()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond("""[{"result":0},{"error":3}]""");
        await client.LoginAsync();

        // Act
        var results = await client.Core.BatchAsync(new[]
        {
            new BatchRequest { Svc = "core/check_unique", Params = new { type = "user", value = "test1" } },
            new BatchRequest { Svc = "core/check_unique", Params = new { type = "user", value = "test2" } },
        });

        // Assert
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task ServiceCall_WithoutLogin_ThrowsInvalidOperation()
    {
        // Arrange
        var (_, client) = MockHttpHandler.CreateClient();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.Core.SearchUnitsAsync("*"));
    }
}

public sealed class MessagesServiceTests
{
    [Fact]
    public async Task LoadLastAsync_ReturnsMessages()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var messagesJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "load_messages.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond(messagesJson);
        await client.LoginAsync();

        // Act
        var result = await client.Messages.LoadLastAsync(34868, lastCount: 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Messages.Count);

        var dataMsg = result.Messages[0];
        Assert.Equal("ud", dataMsg.Type);
        Assert.NotNull(dataMsg.Position);
        Assert.Equal(43.2444668, dataMsg.Position!.Latitude, 5);
        Assert.Equal(-118.1464477, dataMsg.Position.Longitude, 5);
        Assert.Equal(12, dataMsg.Position.Speed);

        var smsMsg = result.Messages[1];
        Assert.Equal("us", smsMsg.Type);
        Assert.Equal("Test SMS message", smsMsg.SmsText);
        Assert.Equal("+79001234567", smsMsg.ModemPhone);
    }

    [Fact]
    public async Task LoadLastAsync_SendsCorrectParams()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var messagesJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "load_messages.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond(messagesJson);
        await client.LoginAsync();

        // Act
        await client.Messages.LoadLastAsync(12345, lastCount: 50);

        // Assert
        var request = handler.LastRequest;
        var rawBody = await request!.Content!.ReadAsStringAsync();
        var body = Uri.UnescapeDataString(rawBody);
        Assert.Contains("messages/load_last", body);
        Assert.Contains("12345", body);
        Assert.Contains("50", body);
    }

    [Fact]
    public async Task LoadIntervalAsync_CorrectParameters()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var messagesJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "load_messages.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond(messagesJson);
        await client.LoginAsync();

        long from = 1614336700;
        long to = 1614336800;

        // Act
        var result = await client.Messages.LoadIntervalAsync(34868, from, to);

        // Assert
        Assert.NotNull(result);
        var request = handler.LastRequest;
        var rawBody = await request!.Content!.ReadAsStringAsync();
        var body = Uri.UnescapeDataString(rawBody);
        Assert.Contains("messages/load_interval", body);
        Assert.Contains("1614336700", body);
        Assert.Contains("1614336800", body);
    }
}

public sealed class TokenServiceTests
{
    [Fact]
    public async Task ListAsync_ReturnsTokens()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond("""[{"h":"abc123","app":"TestApp","at":0,"ct":1700000000,"dur":0,"fl":-1,"items":[]}]""");
        await client.LoginAsync();

        // Act
        var tokens = await client.Tokens.ListAsync();

        // Assert
        Assert.NotNull(tokens);
        Assert.Single(tokens);
        Assert.Equal("abc123", tokens[0].Hash);
        Assert.Equal("TestApp", tokens[0].AppName);
        Assert.Equal(-1, tokens[0].AccessFlags);
    }

    [Fact]
    public async Task DeleteAllAsync_SendsCorrectRequest()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond("{}");
        await client.LoginAsync();

        // Act
        await client.Tokens.DeleteAllAsync();

        // Assert
        var request = handler.LastRequest;
        var rawBody = await request!.Content!.ReadAsStringAsync();
        var body = Uri.UnescapeDataString(rawBody);
        Assert.Contains("token/update", body);
        Assert.Contains("delete", body);
        Assert.Contains("true", body.ToLower());
    }
}
