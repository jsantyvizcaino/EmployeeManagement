using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<EmployeeSalary> EmployeeSalaries => Set<EmployeeSalary>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        SetAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        SetAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditFields()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;

                if (string.IsNullOrWhiteSpace(entry.Entity.CreatedBy))
                    entry.Entity.CreatedBy = DatabaseConstants.SystemUser;

                continue;
            }

            if (entry.State != EntityState.Modified)
                continue;

            entry.Property(entity => entity.CreatedAt).IsModified = false;
            entry.Property(entity => entity.CreatedBy).IsModified = false;
            entry.Entity.UpdatedAt = now;

            if (string.IsNullOrWhiteSpace(entry.Entity.UpdatedBy))
                entry.Entity.UpdatedBy = DatabaseConstants.SystemUser;
        }
    }
}
