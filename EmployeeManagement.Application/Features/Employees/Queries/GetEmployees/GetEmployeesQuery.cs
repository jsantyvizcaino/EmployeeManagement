using EmployeeManagement.Application.Features.Employees.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using Mediator;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetEmployees;

public sealed record GetEmployeesQuery(long? AreaId = null)
    : IQuery<ListResultDto<EmployeeResponseDto>>;
