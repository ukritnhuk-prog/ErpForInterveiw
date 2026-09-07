using Application.Departments.Models;
using MediatR;

namespace Application.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(int Id, DepartmentRequest Data) : IRequest<DepartmentResponse>;
