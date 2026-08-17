namespace Wialon.Sdk.Errors;

/// <summary>
/// Исключение, возникающее при получении ошибки от Wialon API.
/// </summary>
public class WialonException : Exception
{
    /// <summary>Код ошибки Wialon API.</summary>
    public WialonErrorCode ErrorCode { get; }

    /// <summary>Сырой JSON-ответ от сервера (если доступен).</summary>
    public string? RawResponse { get; }

    public WialonException(WialonErrorCode errorCode)
        : base(GetMessage(errorCode))
    {
        ErrorCode = errorCode;
    }

    public WialonException(WialonErrorCode errorCode, string? rawResponse)
        : base(GetMessage(errorCode))
    {
        ErrorCode = errorCode;
        RawResponse = rawResponse;
    }

    public WialonException(WialonErrorCode errorCode, string message, string? rawResponse = null)
        : base(message)
    {
        ErrorCode = errorCode;
        RawResponse = rawResponse;
    }

    private static string GetMessage(WialonErrorCode code) => code switch
    {
        WialonErrorCode.Success              => "Success.",
        WialonErrorCode.InvalidSession       => "Invalid session (sid).",
        WialonErrorCode.InvalidServiceName   => "Invalid API service name.",
        WialonErrorCode.InvalidResult        => "Invalid result / item not found.",
        WialonErrorCode.InvalidInput         => "Invalid input parameters.",
        WialonErrorCode.ExecutionError       => "Execution error.",
        WialonErrorCode.UnknownError         => "Unknown error.",
        WialonErrorCode.AccessDenied         => "Access denied / user disabled.",
        WialonErrorCode.InvalidCredentials   => "Invalid username or password.",
        WialonErrorCode.AuthServerUnavailable=> "Authorization server unavailable.",
        WialonErrorCode.ConcurrentRequestLimit => "Concurrent request limit reached.",
        WialonErrorCode.PasswordResetError   => "Password reset error.",
        WialonErrorCode.BillingError         => "Billing error.",
        WialonErrorCode.NoMessages           => "No messages for the selected interval.",
        WialonErrorCode.DuplicateOrQuotaLimit=> "Duplicate item or quota limit reached.",
        WialonErrorCode.SingleRequestAllowed => "Only one request is allowed / limit exceeded.",
        WialonErrorCode.MessageLimitExceeded => "Message limit exceeded.",
        WialonErrorCode.ExecutionTimeout     => "Execution timeout exceeded.",
        WialonErrorCode.TwoFactorAttemptsLimit => "Two-factor auth attempt limit exceeded.",
        WialonErrorCode.IpChangedOrSessionExpired => "IP changed or session expired.",
        WialonErrorCode.SensorInUse          => "Sensor deletion denied — sensor is used elsewhere.",
        WialonErrorCode.InternalNetworkTimeout  => "Internal error: network timeout.",
        WialonErrorCode.InternalNetworkResponse => "Internal error: invalid network response.",
        _ => $"Wialon API error: {(int)code}."
    };
}
