using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Interfaces.Persistence;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Infrastructure.Persistence.Repositories;

namespace EmployeeManagement.Infrastructure.Persistence;

public sealed class ReadUnitOfWork : IReadUnitOfWork
{
    public ReadUnitOfWork(AppDbContext context)
    {
        Employees = new EmployeeReadRepository(context);
        Areas = new ReadRepositoryBase<Area>(context);
        Positions = new ReadRepositoryBase<Position>(context);
    }

    public IEmployeeReadRepository Employees { get; }
    public IReadRepositoryBase<Area> Areas { get; }
    public IReadRepositoryBase<Position> Positions { get; }
}
