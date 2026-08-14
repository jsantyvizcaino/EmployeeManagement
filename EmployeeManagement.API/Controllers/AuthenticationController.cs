using System.Net.Mime;
using Asp.Versioning;
using EmployeeManagement.Application.Features.Authentication.Commands.Login;
using EmployeeManagement.Application.Features.Authentication.Dtos.Request;
using EmployeeManagement.Application.Features.Authentication.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiVersion("1.0")]
public sealed class AuthenticationController(
    ILoggerFactory loggerFactory,
    IMediator mediator)
    : BaseController(loggerFactory)
{
    [AllowAnonymous]
    [HttpPost("login")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(ResultDto<LoginResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ResultDto<LoginResponseDto>),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ResultDto<LoginResponseDto>),
        StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto dto,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "{Method}: Authenticating user {UserName}",
            nameof(Login),
            dto.UserName);

        var result = await mediator.Send(
            new LoginCommand(dto),
            cancellationToken);

        return HandleResult(result);
    }
}
