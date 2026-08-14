using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces.Security;

public interface IPasswordHasherService
{
    string HashPassword(User user, string password);
    bool VerifyPassword(User user, string passwordHash, string password);
}
