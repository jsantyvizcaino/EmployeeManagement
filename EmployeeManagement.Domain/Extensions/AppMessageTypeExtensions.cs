using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Extensions;

public static class AppMessageTypeExtensions
{
    public static string GetErrorMessage(this AppMessageType messageType) => messageType switch
    {
        AppMessageType.InvalidRequest => "Invalid request",
        AppMessageType.UnknownError => "An unknown error occurred",
        AppMessageType.NotFound => "The resource was not found",
        AppMessageType.ResourceAlreadyExists => "The resource already exists",
        AppMessageType.InvalidCredentials => "The supplied credentials are invalid",
        AppMessageType.Unauthorized => "Unauthorized",
        AppMessageType.Forbidden => "Forbidden",
        _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
    };

    public static string GetErrorCode(this AppMessageType messageType)
        => $"ERR_{(int)messageType}";
}
