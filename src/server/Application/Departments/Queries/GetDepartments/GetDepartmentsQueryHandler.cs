using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Queries.GetDepartments;

public sealed class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, List<DepartmentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<DepartmentDto>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken) =>
        DepartmentProjection.ToDto(_context.Departments.AsNoTracking().OrderBy(department => department.DepartmentName))
            .ToListAsync(cancellationToken);
}
