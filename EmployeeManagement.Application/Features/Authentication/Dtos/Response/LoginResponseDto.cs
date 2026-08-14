namespace EmployeeManagement.Application.Features.Authentication.Dtos.Response;

public sealed record LoginResponseDto(
    string AccessToken,
    string TokenType,
    long UserId,
    long EmployeeId,
    string UserName,
    string FullName);
