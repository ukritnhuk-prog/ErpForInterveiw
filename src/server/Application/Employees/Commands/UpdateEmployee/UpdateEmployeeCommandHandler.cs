using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.UpdateEmployee;

public sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        EmployeeRequestValidator.Validate(request.Data);

        var employee = await _context.Employees
            .SingleOrDefaultAsync(item => item.EmployeeId == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Employee not found.");

        if (!await _context.Departments.AnyAsync(
                department => department.DepartmentId == request.Data.DepartmentId,
                cancellationToken))
            throw new ValidationException("The selected department does not exist.");

        employee.DepartmentId = request.Data.DepartmentId;
        employee.FirstName = request.Data.FirstName.Trim();
        employee.LastName = request.Data.LastName.Trim();
        employee.Gender = request.Data.Gender;
        employee.DateOfBirth = request.Data.DateOfBirth;
        employee.DateJoined = request.Data.DateJoined;
        employee.EmployeeAddress = string.IsNullOrWhiteSpace(request.Data.EmployeeAddress)
            ? null
            : request.Data.EmployeeAddress.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return await EmployeeProjection.ToDto(
                _context.Employees.AsNoTracking().Where(item => item.EmployeeId == employee.EmployeeId))
            .SingleAsync(cancellationToken);
    }
}
