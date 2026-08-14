using EmployeeManagement.Domain.Models;
using FluentValidation;

namespace EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandValidator
    : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator(AppSettings settings)
    {
        RuleFor(command => command.Dto)
            .NotNull()
            .WithMessage("Los datos del empleado son requeridos.");

        When(command => command.Dto is not null, () =>
        {
            RuleFor(command => command.Dto.UserName)
                .NotEmpty()
                .WithMessage("El usuario es requerido.")
                .MaximumLength(100)
                .WithMessage("El usuario no puede superar los 100 caracteres.");

            RuleFor(command => command.Dto.Password)
                .NotEmpty()
                .WithMessage("La contraseña es requerida.")
                .MinimumLength(settings.PasswordMinLength)
                .WithMessage(
                    $"La contraseña debe tener al menos {settings.PasswordMinLength} caracteres.")
                .MaximumLength(settings.PasswordMaxLength)
                .WithMessage(
                    $"La contraseña no puede superar los {settings.PasswordMaxLength} caracteres.");

            RuleFor(command => command.Dto.DocumentNumber)
                .NotEmpty()
                .WithMessage("El número de documento es requerido.")
                .MaximumLength(20)
                .WithMessage("El número de documento no puede superar los 20 caracteres.");

            RuleFor(command => command.Dto.FirstName)
                .NotEmpty()
                .WithMessage("Los nombres son requeridos.")
                .MaximumLength(100)
                .WithMessage("Los nombres no pueden superar los 100 caracteres.");

            RuleFor(command => command.Dto.LastName)
                .NotEmpty()
                .WithMessage("Los apellidos son requeridos.")
                .MaximumLength(100)
                .WithMessage("Los apellidos no pueden superar los 100 caracteres.");

            RuleFor(command => command.Dto.BirthDate)
                .NotEmpty()
                .WithMessage("La fecha de nacimiento es requerida.")
                .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("La fecha de nacimiento debe ser anterior a la fecha actual.");

            RuleFor(command => command.Dto.AreaId)
                .GreaterThan(0)
                .WithMessage("El área seleccionada no es válida.");

            RuleFor(command => command.Dto.PositionId)
                .GreaterThan(0)
                .WithMessage("El cargo seleccionado no es válido.");

            RuleFor(command => command.Dto.MonthlyAmount)
                .GreaterThan(0)
                .WithMessage("El salario mensual debe ser mayor que cero.");
        });
    }
}
