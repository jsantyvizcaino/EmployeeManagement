using EmployeeManagement.Domain.Interfaces.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence.Repositories;

public class RepositoryBase<TEntity>(AppDbContext context)
    : IRepositoryBase<TEntity>
    where TEntity : class, IBaseEntity
{
    protected readonly AppDbContext Context = context;
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public virtual IQueryable<TEntity> Query()
        => DbSet.AsQueryable();

    public virtual Task<List<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
        => DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public virtual Task<TEntity?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
        => DbSet.FirstOrDefaultAsync(
            entity => entity.Id == id,
            cancellationToken);

    public virtual Task<bool> ExistsByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
        => DbSet.AnyAsync(entity => entity.Id == id, cancellationToken);

    public virtual void Add(TEntity entity)
        => DbSet.Add(entity);

    public virtual void AddRange(IEnumerable<TEntity> entities)
        => DbSet.AddRange(entities);

    public virtual void Update(TEntity entity)
        => DbSet.Update(entity);

    public virtual void Remove(TEntity entity)
        => DbSet.Remove(entity);
}
