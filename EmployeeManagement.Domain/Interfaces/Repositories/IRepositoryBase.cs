using EmployeeManagement.Domain.Interfaces.Entities;

namespace EmployeeManagement.Domain.Interfaces.Repositories;

public interface IRepositoryBase<TEntity>
    where TEntity : class, IBaseEntity
{
    IQueryable<TEntity> Query();
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(long id, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
