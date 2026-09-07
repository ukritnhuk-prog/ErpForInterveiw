using Application.Common.Interfaces;
using Application.Departments.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Queries.GetDepartments;

public sealed class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, List<DepartmentResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<DepartmentResponse>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var departments = _context.Departments
            .AsNoTracking()
            .OrderBy(department => department.DepartmentName);

        return DepartmentProjection
            .ToResponse(departments)
            .ToListAsync(cancellationToken);
    }
}
