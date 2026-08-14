using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public abstract class BaseController : ControllerBase
{
    protected BaseController(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType());
    }

    protected ILogger Logger { get; }

    protected IActionResult HandleResult<T>(T result)
        where T : EmptyResultDto
    {
        if (result.Succeed)
            return Ok(result);

        return result.MessageType switch
        {
            AppMessageType.InvalidRequest => BadRequest(result),
            AppMessageType.NotFound => NotFound(result),
            AppMessageType.ResourceAlreadyExists => Conflict(result),
            AppMessageType.InvalidCredentials => Unauthorized(result),
            AppMessageType.Unauthorized => Unauthorized(result),
            AppMessageType.Forbidden => StatusCode(
                StatusCodes.Status403Forbidden,
                result),
            AppMessageType.UnknownError => StatusCode(
                StatusCodes.Status500InternalServerError,
                result),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result)
        };
    }
}
