using Application.Common.Interfaces;
using Application.Departments;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class DepartmentRepository(ApplicationDbContext dbContext) : IDepartmentRepository
{
    private static IQueryable<DepartmentDto> Project(IQueryable<Department> query) => query
        .Select(d => new DepartmentDto(d.DepartmentId, d.DepartmentName, d.DepartmentAddress, d.Employees.Count));

    public Task<List<DepartmentDto>> GetAsync(CancellationToken cancellationToken) =>
        Project(dbContext.Departments.AsNoTracking().OrderBy(d => d.DepartmentName)).ToListAsync(cancellationToken);

    public Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        Project(dbContext.Departments.AsNoTracking().Where(d => d.DepartmentId == id)).FirstOrDefaultAsync(cancellationToken);

    public Task<Department?> FindAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id, cancellationToken);

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Departments.AnyAsync(d => d.DepartmentId == id, cancellationToken);

    public Task<bool> HasEmployeesAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Employees.AnyAsync(e => e.DepartmentId == id, cancellationToken);

    public void Add(Department department) => dbContext.Departments.Add(department);
    public void Remove(Department department) => dbContext.Departments.Remove(department);
    public Task SaveChangesAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
