using System.ComponentModel.DataAnnotations;

namespace Application.Departments;

public record DepartmentDto(int DepartmentId, string DepartmentName, string? DepartmentAddress, int EmployeeCount);

public class DepartmentRequest
{
    [Required, MaxLength(200)]
    public string DepartmentName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? DepartmentAddress { get; set; }
}
