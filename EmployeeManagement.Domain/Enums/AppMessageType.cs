namespace EmployeeManagement.Domain.Enums;

public enum AppMessageType
{
    InvalidRequest = 1,
    UnknownError = 2,
    NotFound = 3,
    ResourceAlreadyExists = 4,
    InvalidCredentials = 5,
    Unauthorized = 6,
    Forbidden = 7
}
