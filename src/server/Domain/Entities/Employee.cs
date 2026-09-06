namespace Domain.Entities;

public class Employee
{
    public int EmployeeId { get; set; }
    public int DepartmentId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public DateOnly DateJoined { get; set; }
    public string? EmployeeAddress { get; set; }
    public string? Photo { get; set; }
    public Department Department { get; set; } = null!;
}
