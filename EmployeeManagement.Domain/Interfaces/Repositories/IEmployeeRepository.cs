using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Interfaces.Repositories;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    IQueryable<Employee> QueryWithDetails();

    Task<Employee?> GetByUserIdAsync(
        long userId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByDocumentNumberAsync(
        string documentNumber,
        CancellationToken cancellationToken = default);
}
