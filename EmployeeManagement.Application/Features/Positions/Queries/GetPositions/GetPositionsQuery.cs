using EmployeeManagement.Application.Features.Positions.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using Mediator;

namespace EmployeeManagement.Application.Features.Positions.Queries.GetPositions;

public sealed record GetPositionsQuery
    : IQuery<ListResultDto<PositionResponseDto>>;
