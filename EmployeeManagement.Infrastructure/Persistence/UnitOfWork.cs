using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Persistence;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeManagement.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        Employees = new EmployeeRepository(context);
        Areas = new RepositoryBase<Area>(context);
        Positions = new RepositoryBase<Position>(context);
        EmployeeSalaries = new RepositoryBase<EmployeeSalary>(context);
    }

    public IUserRepository Users { get; }
    public IEmployeeRepository Employees { get; }
    public IRepositoryBase<Area> Areas { get; }
    public IRepositoryBase<Position> Positions { get; }
    public IRepositoryBase<EmployeeSalary> EmployeeSalaries { get; }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already active.");

        _transaction = await _context.Database.BeginTransactionAsync(
            cancellationToken);
    }

    public async Task CommitTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException("There is no active transaction.");

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
            await _transaction.DisposeAsync();

        await _context.DisposeAsync();
    }
}
