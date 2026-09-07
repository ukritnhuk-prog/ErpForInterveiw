using Application.Common.Interfaces;
using Application.Employees.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Queries.GetEmployee;

public sealed class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeResponse>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeResponse> Handle(GetEmployeeQuery request, CancellationToken cancellationToken) =>
        await EmployeeProjection.ToResponse(
                _context.Employees.AsNoTracking().Where(employee => employee.EmployeeId == request.Id))
            .SingleOrDefaultAsync(cancellationToken)
        ?? throw new KeyNotFoundException("Employee not found.");
}
