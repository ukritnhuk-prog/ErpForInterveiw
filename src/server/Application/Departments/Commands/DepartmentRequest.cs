namespace Application.Departments.Commands;

public class DepartmentRequest
{
    public string DepartmentName { get; set; } = string.Empty;
    public string? DepartmentAddress { get; set; }
}
