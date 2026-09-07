using Application.Common.Interfaces;
using Application.Departments.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Queries.GetDepartment;

public sealed class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, DepartmentResponse>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DepartmentResponse> Handle(GetDepartmentQuery request, CancellationToken cancellationToken) =>
        await DepartmentProjection.ToResponse(
                _context.Departments.AsNoTracking().Where(department => department.DepartmentId == request.Id))
            .SingleOrDefaultAsync(cancellationToken)
        ?? throw new KeyNotFoundException("Department not found.");
}
