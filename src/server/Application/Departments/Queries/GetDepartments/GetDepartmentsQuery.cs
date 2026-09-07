using Application.Departments.Models;
using MediatR;

namespace Application.Departments.Queries.GetDepartments;

public record GetDepartmentsQuery : IRequest<List<DepartmentResponse>>;
