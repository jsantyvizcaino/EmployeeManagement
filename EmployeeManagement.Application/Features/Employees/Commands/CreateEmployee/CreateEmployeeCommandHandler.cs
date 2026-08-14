using EmployeeManagement.Application.Features.Employees.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Persistence;
using EmployeeManagement.Domain.Interfaces.Security;
using Mediator;

namespace EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasherService passwordHasher)
    : ICommandHandler<CreateEmployeeCommand, ResultDto<EmployeeResponseDto>>
{
    public async ValueTask<ResultDto<EmployeeResponseDto>> Handle(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var dto = command.Dto;
        var userName = dto.UserName.Trim();
        var documentNumber = dto.DocumentNumber.Trim();

        var existingUser = await unitOfWork.Users.GetByUserNameAsync(
            userName,
            cancellationToken);
        if (existingUser is not null)
        {
            return Result.ResourceAlreadyExists<EmployeeResponseDto>(
                $"Ya existe un usuario con el nombre '{userName}'.");
        }

        if (await unitOfWork.Employees.ExistsByDocumentNumberAsync(
                documentNumber,
                cancellationToken))
        {
            return Result.ResourceAlreadyExists<EmployeeResponseDto>(
                $"Ya existe un empleado con el documento '{documentNumber}'.");
        }

        var area = await unitOfWork.Areas.GetByIdAsync(
            dto.AreaId,
            cancellationToken);
        if (area is null)
        {
            return Result.NotFound<EmployeeResponseDto>(
                $"No se encontró el área con id '{dto.AreaId}'.");
        }

        var position = await unitOfWork.Positions.GetByIdAsync(
            dto.PositionId,
            cancellationToken);
        if (position is null)
        {
            return Result.NotFound<EmployeeResponseDto>(
                $"No se encontró el cargo con id '{dto.PositionId}'.");
        }

        var user = new User
        {
            UserName = userName,
            IsActive = true
        };
        user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

        var employee = new Employee
        {
            User = user,
            DocumentNumber = documentNumber,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            BirthDate = dto.BirthDate,
            AreaId = area.Id,
            Area = area,
            PositionId = position.Id,
            Position = position
        };
        var salary = new EmployeeSalary
        {
            Employee = employee,
            MonthlyAmount = dto.MonthlyAmount
        };

        user.Employee = employee;
        employee.Salary = salary;

        unitOfWork.Users.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new EmployeeResponseDto(
            employee.Id,
            employee.DocumentNumber,
            employee.FirstName,
            employee.LastName,
            employee.BirthDate,
            CalculateAge(employee.BirthDate),
            area.Id,
            area.Name,
            position.Id,
            position.Name,
            salary.MonthlyAmount);

        return Result.Success(response);
    }

    private static int CalculateAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;
        if (birthDate.AddYears(age) > today)
            age--;

        return age;
    }
}
