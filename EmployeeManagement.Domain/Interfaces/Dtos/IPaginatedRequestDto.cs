namespace EmployeeManagement.Domain.Interfaces.Dtos;

public interface IPaginatedRequestDto
{
    int Page { get; set; }
    int Take { get; set; }
    string? SearchTerm { get; set; }
    string? OrderBy { get; set; }
    bool OrderByAsc { get; set; }
}
