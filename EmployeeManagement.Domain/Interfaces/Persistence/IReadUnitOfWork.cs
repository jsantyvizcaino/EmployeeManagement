using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Repositories;

namespace EmployeeManagement.Domain.Interfaces.Persistence;

public interface IReadUnitOfWork
{
    IEmployeeReadRepository Employees { get; }
    IReadRepositoryBase<Area> Areas { get; }
    IReadRepositoryBase<Position> Positions { get; }
}
