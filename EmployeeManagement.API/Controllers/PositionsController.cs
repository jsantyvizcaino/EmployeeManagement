using System.Net.Mime;
using Asp.Versioning;
using EmployeeManagement.Application.Features.Positions.Dtos.Response;
using EmployeeManagement.Application.Features.Positions.Queries.GetPositions;
using EmployeeManagement.Domain.Dtos;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiVersion("1.0")]
public sealed class PositionsController(
    ILoggerFactory loggerFactory,
    IMediator mediator)
    : BaseController(loggerFactory)
{
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(ListResultDto<PositionResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        Logger.LogInformation("{Method}: Getting positions", nameof(GetAll));

        var result = await mediator.Send(
            new GetPositionsQuery(),
            cancellationToken);

        return HandleResult(result);
    }
}
