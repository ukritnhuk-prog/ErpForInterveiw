namespace Domain.Entities;

public class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? DepartmentAddress { get; set; }
    public ICollection<Employee> Employees { get; set; } = [];
}
