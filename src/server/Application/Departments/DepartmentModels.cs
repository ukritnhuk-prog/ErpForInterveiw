namespace Application.Departments;

public record DepartmentDto(int DepartmentId, string DepartmentName, string? DepartmentAddress, int EmployeeCount);

public class DepartmentRequest
{
    public string DepartmentName { get; set; } = string.Empty;

    public string? DepartmentAddress { get; set; }
}
