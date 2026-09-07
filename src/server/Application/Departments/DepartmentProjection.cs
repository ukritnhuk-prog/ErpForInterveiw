using Application.Departments.Models;
using Domain.Entities;

namespace Application.Departments;

internal static class DepartmentProjection
{
    public static IQueryable<DepartmentResponse> ToResponse(IQueryable<Department> query) =>
        query.Select(department => new DepartmentResponse(
            department.DepartmentId,
            department.DepartmentName,
            department.DepartmentAddress,
            department.Employees.Count));
}
