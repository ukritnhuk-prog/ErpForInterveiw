using Application.Departments.Models;
using MediatR;

namespace Application.Departments.Queries.GetDepartment;

public record GetDepartmentQuery(int Id) : IRequest<DepartmentResponse>;
