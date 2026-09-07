namespace Application.Employees.Commands;

public class EmployeeRequest
{
    public int DepartmentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public DateOnly DateJoined { get; set; }
    public string? EmployeeAddress { get; set; }
}
