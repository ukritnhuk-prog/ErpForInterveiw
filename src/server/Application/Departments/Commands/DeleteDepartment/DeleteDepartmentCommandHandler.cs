using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Commands.DeleteDepartment;

public sealed class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .SingleOrDefaultAsync(item => item.DepartmentId == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Department not found.");

        if (await _context.Employees.AnyAsync(employee => employee.DepartmentId == request.Id, cancellationToken))
            throw new ValidationException("Cannot delete this department because employees are currently assigned to it.");

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
