using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;

namespace EmployeeManagement.Domain.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IUserRepository Users { get; }
    IEmployeeRepository Employees { get; }
    IRepositoryBase<Area> Areas { get; }
    IRepositoryBase<Position> Positions { get; }
    IRepositoryBase<EmployeeSalary> EmployeeSalaries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
