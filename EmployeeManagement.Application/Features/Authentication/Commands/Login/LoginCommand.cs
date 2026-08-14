using EmployeeManagement.Application.Features.Authentication.Dtos.Request;
using EmployeeManagement.Application.Features.Authentication.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using Mediator;

namespace EmployeeManagement.Application.Features.Authentication.Commands.Login;

public sealed record LoginCommand(LoginRequestDto Dto)
    : ICommand<ResultDto<LoginResponseDto>>;
