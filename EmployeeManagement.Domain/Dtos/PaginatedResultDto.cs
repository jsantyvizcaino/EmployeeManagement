using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Interfaces.Dtos;

namespace EmployeeManagement.Domain.Dtos;

public class PaginatedResultDto<T> : ListResultDto<T>, IPaginatedResponseDto
{
    public int Take { get; set; }
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => Take <= 0
        ? 0
        : (int)Math.Ceiling((double)TotalRecords / Take);
    public bool HasNext => CurrentPage < TotalPages;
    public bool HasPrevious => CurrentPage > 1;

    public PaginatedResultDto()
        : base([])
    {
    }

    public PaginatedResultDto(List<T> results)
        : base(results)
    {
    }

    public PaginatedResultDto(AppMessageType messageType, string? details)
        : base(messageType, details)
    {
    }
}

public static class PaginatedResult
{
    public static PaginatedResultDto<T> Success<T>(
        IPaginatedRequestDto request,
        int totalRecords,
        List<T> result)
        => Success(request.Page, request.Take, totalRecords, result);

    public static PaginatedResultDto<T> Success<T>(
        int page,
        int take,
        int totalRecords,
        List<T> result)
        => new(result)
        {
            Take = take,
            CurrentPage = page,
            TotalRecords = totalRecords
        };

    public static PaginatedResultDto<T> Empty<T>(IPaginatedRequestDto request)
        => Success<T>(request, 0, []);

    public static PaginatedResultDto<T> FromOther<T>(EmptyResultDto result)
        => new(result.MessageType ?? AppMessageType.UnknownError, null)
        {
            Message = result.Message,
            MessageId = result.MessageId
        };
}
