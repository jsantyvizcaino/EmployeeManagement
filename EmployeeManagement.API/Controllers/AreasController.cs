using System.Net.Mime;
using Asp.Versioning;
using EmployeeManagement.Application.Features.Areas.Dtos.Response;
using EmployeeManagement.Application.Features.Areas.Queries.GetAreas;
using EmployeeManagement.Domain.Dtos;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiVersion("1.0")]
public sealed class AreasController(
    ILoggerFactory loggerFactory,
    IMediator mediator)
    : BaseController(loggerFactory)
{
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(ListResultDto<AreaResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        Logger.LogInformation("{Method}: Getting areas", nameof(GetAll));

        var result = await mediator.Send(
            new GetAreasQuery(),
            cancellationToken);

        return HandleResult(result);
    }
}
