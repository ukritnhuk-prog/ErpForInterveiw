using MediatR;

namespace Application.Departments.Queries.GetDepartment;

public record GetDepartmentQuery(int Id) : IRequest<DepartmentDto>;
