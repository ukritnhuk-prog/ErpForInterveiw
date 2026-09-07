namespace Application.Employees.Models;

public record EmployeeResponse(
    int EmployeeId,
    int DepartmentId,
    string DepartmentName,
    string FirstName,
    string LastName,
    string Gender,
    DateOnly DateOfBirth,
    DateOnly DateJoined,
    string? EmployeeAddress,
    string? Photo)
{
    public string FullName => $"{FirstName} {LastName}";
}
