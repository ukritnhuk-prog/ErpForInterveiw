using MediatR;

namespace Application.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(EmployeeRequest Data) : IRequest<EmployeeDto>;
