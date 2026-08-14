using EmployeeManagement.Application.Features.Employees.Dtos.Request;
using EmployeeManagement.Application.Features.Employees.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using Mediator;

namespace EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(CreateEmployeeRequestDto Dto)
    : ICommand<ResultDto<EmployeeResponseDto>>;
