using Application.Departments.Models;
using MediatR;

namespace Application.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(DepartmentRequest Data) : IRequest<DepartmentResponse>;
