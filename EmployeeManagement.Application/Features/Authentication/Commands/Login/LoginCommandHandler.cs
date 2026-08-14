using EmployeeManagement.Application.Features.Authentication.Dtos.Response;
using EmployeeManagement.Domain.Dtos;
using EmployeeManagement.Domain.Interfaces.Persistence;
using EmployeeManagement.Domain.Interfaces.Security;
using Mediator;

namespace EmployeeManagement.Application.Features.Authentication.Commands.Login;

public sealed class LoginCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasherService passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator)
    : ICommandHandler<LoginCommand, ResultDto<LoginResponseDto>>
{
    public async ValueTask<ResultDto<LoginResponseDto>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByUserNameAsync(
            command.Dto.UserName.Trim(),
            cancellationToken);

        if (user is null || !user.IsActive || user.Employee is null)
        {
            return Result.InvalidCredentials<LoginResponseDto>(
                "El usuario o la contraseña son incorrectos.");
        }

        if (!passwordHasher.VerifyPassword(
                user,
                user.PasswordHash,
                command.Dto.Password))
        {
            return Result.InvalidCredentials<LoginResponseDto>(
                "El usuario o la contraseña son incorrectos.");
        }

        var employee = user.Employee;
        var accessToken = jwtTokenGenerator.Generate(user, employee);
        var response = new LoginResponseDto(
            accessToken,
            "Bearer",
            user.Id,
            employee.Id,
            user.UserName,
            $"{employee.FirstName} {employee.LastName}".Trim());

        return Result.Success(response);
    }
}
