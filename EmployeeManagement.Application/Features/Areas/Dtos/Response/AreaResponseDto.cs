namespace EmployeeManagement.Application.Features.Areas.Dtos.Response;

public sealed record AreaResponseDto(
    long Id,
    string Name,
    string? Description);
