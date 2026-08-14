using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence.Repositories;

public sealed class EmployeeRepository(AppDbContext context)
    : RepositoryBase<Employee>(context), IEmployeeRepository
{
    public IQueryable<Employee> QueryWithDetails()
        => DbSet
            .Include(employee => employee.User)
            .Include(employee => employee.Area)
            .Include(employee => employee.Position)
            .Include(employee => employee.Salary);

    public Task<Employee?> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken = default)
        => QueryWithDetails().FirstOrDefaultAsync(
            employee => employee.UserId == userId,
            cancellationToken);
}
