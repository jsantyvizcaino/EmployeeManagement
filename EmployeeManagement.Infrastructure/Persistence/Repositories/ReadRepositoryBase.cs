using EmployeeManagement.Domain.Interfaces.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence.Repositories;

public sealed class ReadRepositoryBase<TEntity>(AppDbContext context)
    : IReadRepositoryBase<TEntity>
    where TEntity : class, IBaseEntity
{
    private readonly IQueryable<TEntity> _query = context
        .Set<TEntity>()
        .AsNoTracking();

    public IQueryable<TEntity> Query()
        => _query;

    public Task<List<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
        => _query.ToListAsync(cancellationToken);

    public Task<TEntity?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
        => _query.FirstOrDefaultAsync(
            entity => EF.Property<long>(entity, nameof(IBaseEntity.Id)) == id,
            cancellationToken);

    public Task<bool> ExistsByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
        => _query.AnyAsync(
            entity => EF.Property<long>(entity, nameof(IBaseEntity.Id)) == id,
            cancellationToken);
}
