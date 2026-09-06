using MediatR;

namespace Application.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(string? Search, int? DepartmentId) : IRequest<List<EmployeeDto>>;
