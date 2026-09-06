using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Commands.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .SingleOrDefaultAsync(item => item.EmployeeId == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Employee not found.");

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
