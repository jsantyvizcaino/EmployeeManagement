using EmployeeManagement.Application.Features.Areas.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using Mediator;

namespace EmployeeManagement.Application.Features.Areas.Queries.GetAreas;

public sealed record GetAreasQuery
    : IQuery<ListResultDto<AreaResponseDto>>;
