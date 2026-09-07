namespace Application.Departments.Models;

public record DepartmentResponse(
    int DepartmentId,
    string DepartmentName,
    string? DepartmentAddress,
    int EmployeeCount);
