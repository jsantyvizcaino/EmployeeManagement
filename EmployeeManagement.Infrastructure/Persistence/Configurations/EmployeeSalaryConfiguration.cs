using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

internal sealed class EmployeeSalaryConfiguration
    : BaseEntityConfiguration<EmployeeSalary>
{
    public override void Configure(EntityTypeBuilder<EmployeeSalary> builder)
    {
        base.Configure(builder);

        builder.ToTable(
            "EmployeeSalaries",
            DatabaseConstants.BusinessSchema,
            table => table.HasCheckConstraint(
                "CK_EmployeeSalaries_MonthlyAmount_Positive",
                "[MonthlyAmount] > 0"));

        builder.Property(salary => salary.MonthlyAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasIndex(salary => salary.EmployeeId)
            .IsUnique();

        builder.HasOne(salary => salary.Employee)
            .WithOne(employee => employee.Salary)
            .HasForeignKey<EmployeeSalary>(salary => salary.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
