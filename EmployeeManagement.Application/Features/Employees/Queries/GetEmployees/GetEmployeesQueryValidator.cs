using FluentValidation;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetEmployees;

public sealed class GetEmployeesQueryValidator
    : AbstractValidator<GetEmployeesQuery>
{
    public GetEmployeesQueryValidator()
    {
        RuleFor(query => query.AreaId)
            .GreaterThan(0)
            .When(query => query.AreaId.HasValue)
            .WithMessage("El área seleccionada no es válida.");
    }
}
