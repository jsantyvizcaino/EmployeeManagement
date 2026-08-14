using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces.Security;

public interface IJwtTokenGenerator
{
    string Generate(User user, Employee employee);
}
