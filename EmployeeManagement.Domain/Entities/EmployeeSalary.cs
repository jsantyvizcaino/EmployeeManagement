namespace EmployeeManagement.Domain.Entities;

public class EmployeeSalary : BaseEntity
{
    public long EmployeeId { get; set; }
    public decimal MonthlyAmount { get; set; }

    public Employee Employee { get; set; } = null!;
}
