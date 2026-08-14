namespace EmployeeManagement.Application.Features.Positions.Dtos.Response;

public sealed record PositionResponseDto(
    long Id,
    string Name,
    string? Description);
