using Application.Employees.Models;
using MediatR;

namespace Application.Employees.Queries.GetEmployees;

public record GetEmployeesQuery(string? Search, int? DepartmentId) : IRequest<List<EmployeeResponse>>;
