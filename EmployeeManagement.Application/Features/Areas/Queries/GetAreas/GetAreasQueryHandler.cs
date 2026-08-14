using EmployeeManagement.Application.Features.Areas.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Interfaces.Persistence;
using Mediator;

namespace EmployeeManagement.Application.Features.Areas.Queries.GetAreas;

public sealed class GetAreasQueryHandler(IReadUnitOfWork unitOfWork)
    : IQueryHandler<GetAreasQuery, ListResultDto<AreaResponseDto>>
{
    public async ValueTask<ListResultDto<AreaResponseDto>> Handle(
        GetAreasQuery query,
        CancellationToken cancellationToken)
    {
        var areas = await unitOfWork.Areas.GetAllAsync(cancellationToken);
        var response = areas
            .OrderBy(area => area.Name)
            .Select(area => new AreaResponseDto(
                area.Id,
                area.Name,
                area.Description))
            .ToList();

        return ListResult.Success(response);
    }
}
