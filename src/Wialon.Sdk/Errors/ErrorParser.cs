using System.Text.Json;

namespace Wialon.Sdk.Errors;

/// <summary>
/// Парсит JSON-ответ от Wialon и выбрасывает <see cref="WialonException"/> при наличии ошибки.
/// </summary>
public static class ErrorParser
{
    /// <summary>
    /// Проверяет JSON-строку на наличие поля "error" и выбрасывает исключение если оно есть.
    /// </summary>
    /// <param name="json">JSON-строка ответа от Wialon API.</param>
    /// <exception cref="WialonException">Если ответ содержит поле "error".</exception>
    public static void ThrowIfError(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return;

        // Быстрая проверка без полного парсинга
        if (!json.Contains("\"error\""))
            return;

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("error", out var errorProp))
            {
                var code = errorProp.GetInt32();
                var errorCode = Enum.IsDefined(typeof(WialonErrorCode), code)
                    ? (WialonErrorCode)code
                    : WialonErrorCode.UnknownError;

                throw new WialonException(errorCode, json);
            }
        }
        catch (JsonException)
        {
            // Не валидный JSON — не бросаем ошибку парсинга, пусть вызывающий код разберётся
        }
    }

    /// <summary>
    /// Проверяет JsonElement на наличие поля "error".
    /// </summary>
    public static void ThrowIfError(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("error", out var errorProp))
        {
            var code = errorProp.GetInt32();
            var errorCode = Enum.IsDefined(typeof(WialonErrorCode), code)
                ? (WialonErrorCode)code
                : WialonErrorCode.UnknownError;

            throw new WialonException(errorCode);
        }
    }

    /// <summary>
    /// Пытается получить код ошибки из JSON без выброса исключения.
    /// </summary>
    public static bool TryGetErrorCode(string json, out WialonErrorCode errorCode)
    {
        errorCode = WialonErrorCode.Success;
        if (string.IsNullOrWhiteSpace(json) || !json.Contains("\"error\""))
            return false;

        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("error", out var errorProp))
            {
                var code = errorProp.GetInt32();
                errorCode = Enum.IsDefined(typeof(WialonErrorCode), code)
                    ? (WialonErrorCode)code
                    : WialonErrorCode.UnknownError;
                return true;
            }
        }
        catch (JsonException) { }

        return false;
    }
}
