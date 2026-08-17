using Wialon.Sdk.Errors;

namespace Wialon.Sdk.Tests.Errors;

public sealed class ErrorParserTests
{
    [Fact]
    public void ThrowIfError_NoErrorField_DoesNotThrow()
    {
        // Arrange
        var json = """{"result":0}""";

        // Act & Assert
        ErrorParser.ThrowIfError(json); // should not throw
    }

    [Theory]
    [InlineData("""{"error":1}""", WialonErrorCode.InvalidSession)]
    [InlineData("""{"error":2}""", WialonErrorCode.InvalidServiceName)]
    [InlineData("""{"error":4}""", WialonErrorCode.InvalidInput)]
    [InlineData("""{"error":7}""", WialonErrorCode.AccessDenied)]
    [InlineData("""{"error":8}""", WialonErrorCode.InvalidCredentials)]
    [InlineData("""{"error":1001}""", WialonErrorCode.NoMessages)]
    [InlineData("""{"error":1003}""", WialonErrorCode.SingleRequestAllowed)]
    [InlineData("""{"error":1004}""", WialonErrorCode.MessageLimitExceeded)]
    [InlineData("""{"error":2001}""", WialonErrorCode.InvalidItemOrTargetResource)]
    public void ThrowIfError_KnownErrorCode_ThrowsWithCorrectCode(string json, WialonErrorCode expected)
    {
        // Act & Assert
        var ex = Assert.Throws<WialonException>(() => ErrorParser.ThrowIfError(json));
        Assert.Equal(expected, ex.ErrorCode);
    }

    [Fact]
    public void ThrowIfError_UnknownErrorCode_ThrowsUnknownError()
    {
        // Arrange
        var json = """{"error":9999}""";

        // Act & Assert
        var ex = Assert.Throws<WialonException>(() => ErrorParser.ThrowIfError(json));
        Assert.Equal(WialonErrorCode.UnknownError, ex.ErrorCode);
    }

    [Fact]
    public void ThrowIfError_StoresRawResponse()
    {
        // Arrange
        var json = """{"error":7}""";

        // Act & Assert
        var ex = Assert.Throws<WialonException>(() => ErrorParser.ThrowIfError(json));
        Assert.Equal(json, ex.RawResponse);
    }

    [Fact]
    public void ThrowIfError_EmptyString_DoesNotThrow()
    {
        ErrorParser.ThrowIfError(string.Empty);
        ErrorParser.ThrowIfError("   ");
    }

    [Fact]
    public void ThrowIfError_InvalidJson_DoesNotThrow()
    {
        // Invalid JSON should not throw — let caller deal with it
        ErrorParser.ThrowIfError("not json at all");
    }

    [Fact]
    public void TryGetErrorCode_WithError_ReturnsTrueAndCode()
    {
        // Arrange
        var json = """{"error":7}""";

        // Act
        var found = ErrorParser.TryGetErrorCode(json, out var code);

        // Assert
        Assert.True(found);
        Assert.Equal(WialonErrorCode.AccessDenied, code);
    }

    [Fact]
    public void TryGetErrorCode_NoError_ReturnsFalse()
    {
        // Arrange
        var json = """{"result":0}""";

        // Act
        var found = ErrorParser.TryGetErrorCode(json, out var code);

        // Assert
        Assert.False(found);
        Assert.Equal(WialonErrorCode.Success, code);
    }
}

public sealed class WialonExceptionTests
{
    [Fact]
    public void Constructor_WithErrorCode_SetsProperties()
    {
        var ex = new WialonException(WialonErrorCode.AccessDenied);

        Assert.Equal(WialonErrorCode.AccessDenied, ex.ErrorCode);
        Assert.Contains("Access denied", ex.Message);
        Assert.Null(ex.RawResponse);
    }

    [Fact]
    public void Constructor_WithRawResponse_SetsRawResponse()
    {
        var raw = """{"error":7}""";
        var ex = new WialonException(WialonErrorCode.AccessDenied, raw);

        Assert.Equal(WialonErrorCode.AccessDenied, ex.ErrorCode);
        Assert.Equal(raw, ex.RawResponse);
    }

    [Fact]
    public void Constructor_WithCustomMessage_UsesCustomMessage()
    {
        var ex = new WialonException(WialonErrorCode.InvalidInput, "Custom error message", null);

        Assert.Equal("Custom error message", ex.Message);
        Assert.Equal(WialonErrorCode.InvalidInput, ex.ErrorCode);
    }

    [Theory]
    [InlineData(WialonErrorCode.InvalidSession, "Invalid session")]
    [InlineData(WialonErrorCode.InvalidInput, "Invalid input")]
    [InlineData(WialonErrorCode.NoMessages, "No messages")]
    [InlineData(WialonErrorCode.ExecutionTimeout, "timeout")]
    public void Constructor_KnownCodes_HaveDescriptiveMessages(WialonErrorCode code, string expectedFragment)
    {
        var ex = new WialonException(code);

        Assert.Contains(expectedFragment, ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}
