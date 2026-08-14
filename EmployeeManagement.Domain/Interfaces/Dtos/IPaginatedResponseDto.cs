namespace EmployeeManagement.Domain.Interfaces.Dtos;

public interface IPaginatedResponseDto
{
    int Take { get; set; }
    int TotalRecords { get; set; }
    int CurrentPage { get; set; }
    int Records { get; }
    int TotalPages { get; }
    bool HasNext { get; }
    bool HasPrevious { get; }
}
