using Application.Employees;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IEmployeeRepository
{
    Task<List<EmployeeDto>> GetAsync(string? search, int? departmentId, CancellationToken cancellationToken);
    Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Employee?> FindAsync(int id, CancellationToken cancellationToken);
    void Add(Employee employee);
    void Remove(Employee employee);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
