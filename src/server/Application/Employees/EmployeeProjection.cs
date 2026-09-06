using Domain.Entities;

namespace Application.Employees;

internal static class EmployeeProjection
{
    public static IQueryable<EmployeeDto> ToDto(IQueryable<Employee> query) =>
        query.Select(employee => new EmployeeDto(
            employee.EmployeeId,
            employee.DepartmentId,
            employee.Department.DepartmentName,
            employee.FirstName,
            employee.LastName,
            employee.Gender,
            employee.DateOfBirth,
            employee.DateJoined,
            employee.EmployeeAddress,
            employee.Photo));
}
