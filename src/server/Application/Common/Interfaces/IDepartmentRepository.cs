using Application.Departments;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IDepartmentRepository
{
    Task<List<DepartmentDto>> GetAsync(CancellationToken cancellationToken);
    Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Department?> FindAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> HasEmployeesAsync(int id, CancellationToken cancellationToken);
    void Add(Department department);
    void Remove(Department department);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
