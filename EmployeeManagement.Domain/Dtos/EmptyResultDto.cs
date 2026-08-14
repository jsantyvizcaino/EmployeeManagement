using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Extensions;

namespace EmployeeManagement.Domain.Dtos;

public class EmptyResultDto
{
    public bool Succeed { get; set; }
    public string? Message { get; set; }
    public string? MessageId { get; set; }
    public AppMessageType? MessageType { get; set; }

    public EmptyResultDto()
    {
    }

    public EmptyResultDto(bool succeed)
    {
        Succeed = succeed;
    }

    public EmptyResultDto(AppMessageType messageType)
    {
        Succeed = false;
        Message = messageType.GetErrorMessage();
        MessageId = messageType.GetErrorCode();
        MessageType = messageType;
    }

    public EmptyResultDto(AppMessageType messageType, string? details)
        : this(messageType)
    {
        AppendDetails(details);
    }

    public void AppendDetails(string? details)
    {
        if (string.IsNullOrWhiteSpace(details))
            return;

        Message = string.IsNullOrWhiteSpace(Message)
            ? details
            : $"{Message}. {details}";
    }
}

public static class EmptyResult
{
    public static EmptyResultDto Success()
        => new(true);

    public static EmptyResultDto InvalidRequest(string details)
        => new(AppMessageType.InvalidRequest, details);

    public static EmptyResultDto ResourceAlreadyExists(string details)
        => new(AppMessageType.ResourceAlreadyExists, details);

    public static EmptyResultDto NotFound(string details)
        => new(AppMessageType.NotFound, details);

    public static EmptyResultDto UnknownError(string details)
        => new(AppMessageType.UnknownError, details);

    public static EmptyResultDto InvalidCredentials(string details)
        => new(AppMessageType.InvalidCredentials, details);

    public static EmptyResultDto Unauthorized(string details)
        => new(AppMessageType.Unauthorized, details);

    public static EmptyResultDto Forbidden(string details)
        => new(AppMessageType.Forbidden, details);

    public static EmptyResultDto FromOther(EmptyResultDto result)
        => new(result.MessageType ?? AppMessageType.UnknownError)
        {
            Message = result.Message,
            MessageId = result.MessageId
        };

    public static EmptyResultDto InvalidId(long id)
        => InvalidRequest($"The provided id '{id}' is not valid");
}
