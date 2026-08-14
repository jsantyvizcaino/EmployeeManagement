namespace EmployeeManagement.Application.Features.Authentication.Dtos.Request;

public sealed record LoginRequestDto(
    string UserName,
    string Password);
