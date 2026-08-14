using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Features.Employees.Dtos.Response;

public sealed record EmployeeResponseDto(
    long Id,
    string DocumentNumber,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    int Age,
    long AreaId,
    string AreaName,
    long PositionId,
    string PositionName,
    decimal MonthlyAmount)
{
    public static EmployeeResponseDto FromReadModel(EmployeeReadModel employee)
        => new(
            employee.Id,
            employee.DocumentNumber,
            employee.FirstName,
            employee.LastName,
            employee.BirthDate,
            employee.Age,
            employee.AreaId,
            employee.AreaName,
            employee.PositionId,
            employee.PositionName,
            employee.MonthlyAmount);
}
