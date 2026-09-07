using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Employees.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        EmployeeRequestValidator.Validate(request.Data);

        if (!await _context.Departments.AnyAsync(
                department => department.DepartmentId == request.Data.DepartmentId,
                cancellationToken))
            throw new ValidationException("The selected department does not exist.");

        var employee = new Employee();
        Apply(request.Data, employee);

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return await EmployeeProjection.ToResponse(
                _context.Employees.AsNoTracking().Where(item => item.EmployeeId == employee.EmployeeId))
            .SingleAsync(cancellationToken);
    }

    private static void Apply(EmployeeRequest request, Employee employee)
    {
        employee.DepartmentId = request.DepartmentId;
        employee.FirstName = request.FirstName.Trim();
        employee.LastName = request.LastName.Trim();
        employee.Gender = request.Gender;
        employee.DateOfBirth = request.DateOfBirth;
        employee.DateJoined = request.DateJoined;
        employee.EmployeeAddress = string.IsNullOrWhiteSpace(request.EmployeeAddress)
            ? null
            : request.EmployeeAddress.Trim();
    }
}
