using MediatR;

namespace Application.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(int Id, EmployeeRequest Data) : IRequest<EmployeeDto>;
