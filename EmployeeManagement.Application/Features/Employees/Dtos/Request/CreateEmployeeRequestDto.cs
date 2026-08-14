namespace EmployeeManagement.Application.Features.Employees.Dtos.Request;

public sealed record CreateEmployeeRequestDto(
    string UserName,
    string Password,
    string DocumentNumber,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    long AreaId,
    long PositionId,
    decimal MonthlyAmount);
