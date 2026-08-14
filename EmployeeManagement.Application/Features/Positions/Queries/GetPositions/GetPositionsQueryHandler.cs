using EmployeeManagement.Application.Features.Positions.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Interfaces.Persistence;
using Mediator;

namespace EmployeeManagement.Application.Features.Positions.Queries.GetPositions;

public sealed class GetPositionsQueryHandler(IReadUnitOfWork unitOfWork)
    : IQueryHandler<GetPositionsQuery, ListResultDto<PositionResponseDto>>
{
    public async ValueTask<ListResultDto<PositionResponseDto>> Handle(
        GetPositionsQuery query,
        CancellationToken cancellationToken)
    {
        var positions = await unitOfWork.Positions.GetAllAsync(
            cancellationToken);
        var response = positions
            .OrderBy(position => position.Name)
            .Select(position => new PositionResponseDto(
                position.Id,
                position.Name,
                position.Description))
            .ToList();

        return ListResult.Success(response);
    }
}
