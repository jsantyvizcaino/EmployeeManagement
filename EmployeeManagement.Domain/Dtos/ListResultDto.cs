using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Dtos;

public class ListResultDto<T> : ResultDto<List<T>>
{
    public int Records => Result?.Count ?? 0;

    public ListResultDto()
        : this([])
    {
    }

    public ListResultDto(List<T> results)
        : base(results)
    {
    }

    public ListResultDto(AppMessageType messageType, string? details)
        : base(messageType, details)
    {
        Result = [];
    }
}

public static class ListResult
{
    public static ListResultDto<T> Success<T>(List<T> result)
        => new(result);

    public static ListResultDto<T> NotFound<T>(string details)
        => new(AppMessageType.NotFound, details);

    public static ListResultDto<T> FromOther<T>(EmptyResultDto result)
        => new(result.MessageType ?? AppMessageType.UnknownError, null)
        {
            Message = result.Message,
            MessageId = result.MessageId
        };
}
