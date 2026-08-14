using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Security;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.Infrastructure.Security;

public sealed class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(User user, string password)
        => _passwordHasher.HashPassword(user, password);

    public bool VerifyPassword(
        User user,
        string passwordHash,
        string password)
        => _passwordHasher.VerifyHashedPassword(
                user,
                passwordHash,
                password)
            is not PasswordVerificationResult.Failed;
}
