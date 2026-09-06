using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Queries.GetEmployee;

public sealed class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeDto>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> Handle(GetEmployeeQuery request, CancellationToken cancellationToken) =>
        await EmployeeProjection.ToDto(
                _context.Employees.AsNoTracking().Where(employee => employee.EmployeeId == request.Id))
            .SingleOrDefaultAsync(cancellationToken)
        ?? throw new KeyNotFoundException("Employee not found.");
}
