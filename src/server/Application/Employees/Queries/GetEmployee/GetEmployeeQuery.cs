using Application.Employees.Models;
using MediatR;

namespace Application.Employees.Queries.GetEmployee;

public record GetEmployeeQuery(int Id) : IRequest<EmployeeResponse>;
