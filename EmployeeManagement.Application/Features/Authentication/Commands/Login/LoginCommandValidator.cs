using EmployeeManagement.Domain.Models;
using FluentValidation;

namespace EmployeeManagement.Application.Features.Authentication.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(AppSettings settings)
    {
        RuleFor(command => command.Dto)
            .NotNull()
            .WithMessage("Los datos de autenticación son requeridos.");

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
        });
    }
}
