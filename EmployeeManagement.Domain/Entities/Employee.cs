namespace EmployeeManagement.Domain.Entities;

public class Employee : BaseEntity
{
    public long UserId { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public long AreaId { get; set; }
    public long PositionId { get; set; }

    public User User { get; set; } = null!;
    public Area Area { get; set; } = null!;
    public Position Position { get; set; } = null!;
    public EmployeeSalary Salary { get; set; } = null!;
}
