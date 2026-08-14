using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Extensions;

namespace EmployeeManagement.Domain.Dtos;

public class ResultDto<T> : EmptyResultDto
{
    public T? Result { get; set; }

    public ResultDto()
    {
    }

    public ResultDto(T? result)
        : base(true)
    {
        Result = result;
    }

    public ResultDto(AppMessageType messageType, string? details)
        : base(messageType, details)
    {
    }
}

public static class Result
{
    public static ResultDto<T> Success<T>(T? result)
        => new(result);

    public static ResultDto<T> InvalidRequest<T>(string details)
        => new(AppMessageType.InvalidRequest, details);

    public static ResultDto<T> ResourceAlreadyExists<T>(string details)
        => new(AppMessageType.ResourceAlreadyExists, details);

    public static ResultDto<T> NotFound<T>(string details)
        => new(AppMessageType.NotFound, details);

    public static ResultDto<T> UnknownError<T>(string details)
        => new(AppMessageType.UnknownError, details);

    public static ResultDto<T> InvalidCredentials<T>(string details)
        => new(AppMessageType.InvalidCredentials, details);

    public static ResultDto<T> Unauthorized<T>(string details)
        => new(AppMessageType.Unauthorized, details);

    public static ResultDto<T> Forbidden<T>(string details)
        => new(AppMessageType.Forbidden, details);

    public static ResultDto<T> FromOther<T>(EmptyResultDto result)
        => new(result.MessageType ?? AppMessageType.UnknownError, null)
        {
            Message = result.Message,
            MessageId = result.MessageId
        };

    public static ResultDto<T> InvalidId<T>(long id)
        => InvalidRequest<T>($"The provided id '{id}' is not valid");
}
