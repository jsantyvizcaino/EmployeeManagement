using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations;

internal abstract class BaseEntityConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id)
            .ValueGeneratedOnAdd();

        builder.Property(entity => entity.CreatedBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(entity => entity.CreatedAt)
            .HasColumnType("datetime2")
            .IsRequired();

        builder.Property(entity => entity.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(entity => entity.UpdatedAt)
            .HasColumnType("datetime2");
    }
}
