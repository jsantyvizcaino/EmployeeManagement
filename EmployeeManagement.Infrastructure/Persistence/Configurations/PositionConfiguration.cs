using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

internal sealed class PositionConfiguration : BaseEntityConfiguration<Position>
{
    public override void Configure(EntityTypeBuilder<Position> builder)
    {
        base.Configure(builder);

        builder.ToTable("Positions", DatabaseConstants.BusinessSchema);

        builder.Property(position => position.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(position => position.Description)
            .HasMaxLength(250);
    }
}
