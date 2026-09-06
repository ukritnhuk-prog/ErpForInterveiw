using Domain.Entities;

namespace Application.Departments;

internal static class DepartmentProjection
{
    public static IQueryable<DepartmentDto> ToDto(IQueryable<Department> query) =>
        query.Select(department => new DepartmentDto(
            department.DepartmentId,
            department.DepartmentName,
            department.DepartmentAddress,
            department.Employees.Count));
}
