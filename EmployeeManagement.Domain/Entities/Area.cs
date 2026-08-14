namespace EmployeeManagement.Domain.Entities;

public class Area : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Employee> Employees { get; set; } = [];
}
