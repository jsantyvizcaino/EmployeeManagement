using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(AppDbContext context)
    : RepositoryBase<User>(context), IUserRepository
{
    public Task<User?> GetByUserNameAsync(
        string userName,
        CancellationToken cancellationToken = default)
        => DbSet
            .Include(user => user.Employee)
            .FirstOrDefaultAsync(
                user => user.UserName == userName,
                cancellationToken);
}
