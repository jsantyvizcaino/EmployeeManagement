using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

internal sealed class AreaConfiguration : BaseEntityConfiguration<Area>
{
    public override void Configure(EntityTypeBuilder<Area> builder)
    {
        base.Configure(builder);

        builder.ToTable("Areas", DatabaseConstants.BusinessSchema);

        builder.Property(area => area.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(area => area.Description)
            .HasMaxLength(250);
    }
}
