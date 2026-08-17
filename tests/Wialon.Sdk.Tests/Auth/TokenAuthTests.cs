using Wialon.Sdk.Errors;
using Wialon.Sdk.Tests.Fixtures;

namespace Wialon.Sdk.Tests.Auth;

public sealed class TokenAuthTests
{
    [Fact]
    public async Task LoginAsync_ValidToken_ReturnsSession()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);

        // Act
        var session = await client.LoginAsync();

        // Assert
        Assert.NotNull(session);
        Assert.Equal("abc123def456abc123def456abc123de", session.Eid);
        Assert.NotNull(session.User);
        Assert.Equal("test_user", session.User.Name);
        Assert.Equal(648548, session.User.Id);
    }

    [Fact]
    public async Task LoginAsync_SetsSessionIdOnAllServices()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);

        // Act
        var session = await client.LoginAsync();

        // Assert
        Assert.Equal(session.Eid, client.Core.SessionId);
        Assert.Equal(session.Eid, client.Messages.SessionId);
        Assert.Equal(session.Eid, client.Reports.SessionId);
        Assert.Equal(session.Eid, client.Resources.SessionId);
    }

    [Fact]
    public async Task LoginAsync_InvalidToken_ThrowsWialonException()
    {
        // Arrange
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.RespondWithError(7); // Access denied

        // Act & Assert
        var ex = await Assert.ThrowsAsync<WialonException>(() => client.LoginAsync());
        Assert.Equal(WialonErrorCode.AccessDenied, ex.ErrorCode);
    }

    [Fact]
    public async Task LogoutAsync_ClearsSession()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient();
        handler.Respond(loginJson);
        handler.Respond("{}"); // logout response

        await client.LoginAsync();

        // Act
        await client.LogoutAsync();

        // Assert
        Assert.Null(client.CurrentSession);
        Assert.Null(client.Core.SessionId);
    }

    [Fact]
    public async Task LoginAsync_SendsCorrectSvcAndToken()
    {
        // Arrange
        var loginJson = File.ReadAllText(
            Path.Combine("Fixtures", "SampleResponses", "token_login.json"));
        var (handler, client) = MockHttpHandler.CreateClient("my_test_token_72chars_pad00000000000000000000000000000000000000000000000");
        handler.Respond(loginJson);

        // Act
        await client.LoginAsync();

        // Assert
        var request = handler.LastRequest;
        Assert.NotNull(request);
        var rawBody = await request!.Content!.ReadAsStringAsync();
        var body = Uri.UnescapeDataString(rawBody);
        Assert.Contains("token/login", body);
        Assert.Contains("my_test_token", body);
    }
}
