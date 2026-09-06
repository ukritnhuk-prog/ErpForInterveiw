using Application.Common.Interfaces;
using Application.Employees;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class EmployeeRepository(ApplicationDbContext dbContext) : IEmployeeRepository
{
    private static IQueryable<EmployeeDto> Project(IQueryable<Employee> query) => query
        .Select(e => new EmployeeDto(e.EmployeeId, e.DepartmentId, e.Department.DepartmentName,
            e.FirstName, e.LastName, e.Gender, e.DateOfBirth, e.DateJoined, e.EmployeeAddress, e.Photo));

    public Task<List<EmployeeDto>> GetAsync(string? search, int? departmentId, CancellationToken cancellationToken)
    {
        var query = dbContext.Employees.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(e => e.FirstName.Contains(term) || e.LastName.Contains(term)
                || (e.FirstName + " " + e.LastName).Contains(term) || e.Department.DepartmentName.Contains(term));
        }
        if (departmentId.HasValue)
            query = query.Where(e => e.DepartmentId == departmentId.Value);

        return Project(query.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ThenBy(e => e.EmployeeId))
            .ToListAsync(cancellationToken);
    }

    public Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        Project(dbContext.Employees.AsNoTracking().Where(e => e.EmployeeId == id)).FirstOrDefaultAsync(cancellationToken);

    public Task<Employee?> FindAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id, cancellationToken);

    public void Add(Employee employee) => dbContext.Employees.Add(employee);
    public void Remove(Employee employee) => dbContext.Employees.Remove(employee);
    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
