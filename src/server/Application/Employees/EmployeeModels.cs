namespace Application.Employees;

public record EmployeeDto(
    int EmployeeId, int DepartmentId, string DepartmentName,
    string FirstName, string LastName, string Gender,
    DateOnly DateOfBirth, DateOnly DateJoined, string? EmployeeAddress, string? Photo)
{
    public string FullName => $"{FirstName} {LastName}";
}

public class EmployeeRequest
{
    public int DepartmentId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }
    public DateOnly DateJoined { get; set; }

    public string? EmployeeAddress { get; set; }

    // Photo upload is optional and intentionally omitted in this demo.
    // Photo remains a nullable path on the entity and read DTO.
}
