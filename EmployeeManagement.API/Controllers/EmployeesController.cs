using System.Net.Mime;
using Asp.Versioning;
using EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManagement.Application.Features.Employees.Dtos.Request;
using EmployeeManagement.Application.Features.Employees.Dtos.Response;
using EmployeeManagement.Application.Features.Employees.Queries.GetEmployees;
using EmployeeManagement.Domain.Dtos;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiVersion("1.0")]
public sealed class EmployeesController(
    ILoggerFactory loggerFactory,
    IMediator mediator)
    : BaseController(loggerFactory)
{
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(ListResultDto<EmployeeResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ListResultDto<EmployeeResponseDto>),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(
        [FromQuery] long? areaId,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "{Method}: Getting employees. AreaId={AreaId}",
            nameof(GetAll),
            areaId);

        var result = await mediator.Send(
            new GetEmployeesQuery(areaId),
            cancellationToken);

        return HandleResult(result);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(ResultDto<EmployeeResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ResultDto<EmployeeResponseDto>),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ResultDto<EmployeeResponseDto>),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ResultDto<EmployeeResponseDto>),
        StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeRequestDto dto,
        CancellationToken cancellationToken)
    {
        Logger.LogInformation(
            "{Method}: Creating employee with document {DocumentNumber}",
            nameof(Create),
            dto.DocumentNumber);

        var result = await mediator.Send(
            new CreateEmployeeCommand(dto),
            cancellationToken);

        return HandleResult(result);
    }
}
