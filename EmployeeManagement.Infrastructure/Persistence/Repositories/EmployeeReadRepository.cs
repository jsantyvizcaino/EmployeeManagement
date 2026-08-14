using System.Data;
using EmployeeManagement.Domain.Interfaces.Repositories;
using EmployeeManagement.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence.Repositories;

public sealed class EmployeeReadRepository(AppDbContext context)
    : IEmployeeReadRepository
{
    public Task<List<EmployeeReadModel>> GetFromStoredProcedureAsync(
        long? areaId,
        CancellationToken cancellationToken = default)
    {
        var areaIdParameter = new SqlParameter(
            "@AreaId",
            SqlDbType.BigInt)
        {
            Value = areaId.HasValue ? areaId.Value : DBNull.Value
        };

        return context.Database
            .SqlQueryRaw<EmployeeReadModel>(
                "EXEC [pro].[GetEmployees] @AreaId",
                areaIdParameter)
            .ToListAsync(cancellationToken);
    }
}
