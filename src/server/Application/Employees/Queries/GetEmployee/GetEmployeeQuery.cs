using MediatR;

namespace Application.Employees.Queries.GetEmployee;

public record GetEmployeeQuery(int Id) : IRequest<EmployeeDto>;
