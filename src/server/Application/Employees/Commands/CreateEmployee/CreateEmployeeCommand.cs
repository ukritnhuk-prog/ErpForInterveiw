using Application.Employees.Models;
using MediatR;

namespace Application.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(EmployeeRequest Data) : IRequest<EmployeeResponse>;
