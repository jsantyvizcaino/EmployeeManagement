using EmployeeManagement.Domain.Interfaces.Entities;

namespace EmployeeManagement.Domain.Interfaces.Repositories;

public interface IReadRepositoryBase<TEntity>
    where TEntity : class, IBaseEntity
{
    IQueryable<TEntity> Query();
    Task<List<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(
        long id,
        CancellationToken cancellationToken = default);
}
