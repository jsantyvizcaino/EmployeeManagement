using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

internal sealed class EmployeeConfiguration : BaseEntityConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        base.Configure(builder);

        builder.ToTable("Employees", DatabaseConstants.BusinessSchema);

        builder.Property(employee => employee.DocumentNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(employee => employee.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(employee => employee.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(employee => employee.BirthDate)
            .HasColumnType("date")
            .IsRequired();

        builder.HasIndex(employee => employee.DocumentNumber)
            .IsUnique();

        builder.HasIndex(employee => employee.UserId)
            .IsUnique();

        builder.HasOne(employee => employee.User)
            .WithOne(user => user.Employee)
            .HasForeignKey<Employee>(employee => employee.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(employee => employee.Area)
            .WithMany(area => area.Employees)
            .HasForeignKey(employee => employee.AreaId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(employee => employee.Position)
            .WithMany(position => position.Employees)
            .HasForeignKey(employee => employee.PositionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
