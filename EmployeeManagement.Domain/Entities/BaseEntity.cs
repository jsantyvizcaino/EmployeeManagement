using EmployeeManagement.Domain.Interfaces.Entities;

namespace EmployeeManagement.Domain.Entities;

public abstract class BaseEntity : IBaseEntity
{
    public long Id { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
