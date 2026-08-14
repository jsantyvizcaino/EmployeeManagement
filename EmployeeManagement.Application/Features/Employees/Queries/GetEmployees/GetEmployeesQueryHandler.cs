using EmployeeManagement.Application.Features.Employees.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Interfaces.Persistence;
using Mediator;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetEmployees;

public sealed class GetEmployeesQueryHandler(IReadUnitOfWork unitOfWork)
    : IQueryHandler<GetEmployeesQuery, ListResultDto<EmployeeResponseDto>>
{
    public async ValueTask<ListResultDto<EmployeeResponseDto>> Handle(
        GetEmployeesQuery query,
        CancellationToken cancellationToken)
    {
        var employees = await unitOfWork.Employees
            .GetFromStoredProcedureAsync(
                query.AreaId,
                cancellationToken);
        var response = employees
            .Select(EmployeeResponseDto.FromReadModel)
            .ToList();

        return ListResult.Success(response);
    }
}
