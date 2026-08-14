namespace EmployeeManagement.Domain.Models;

public sealed class EmployeeReadModel
{
    public long Id { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public int Age { get; set; }
    public long AreaId { get; set; }
    public string AreaName { get; set; } = string.Empty;
    public long PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public decimal MonthlyAmount { get; set; }
}
