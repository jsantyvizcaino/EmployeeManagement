using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Domain.Interfaces.Repositories;

public interface IEmployeeReadRepository
{
    Task<List<EmployeeReadModel>> GetFromStoredProcedureAsync(
        long? areaId,
        CancellationToken cancellationToken = default);
}
