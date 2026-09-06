using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Queries.GetDepartment;

public sealed class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, DepartmentDto>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DepartmentDto> Handle(GetDepartmentQuery request, CancellationToken cancellationToken) =>
        await DepartmentProjection.ToDto(
                _context.Departments.AsNoTracking().Where(department => department.DepartmentId == request.Id))
            .SingleOrDefaultAsync(cancellationToken)
        ?? throw new KeyNotFoundException("Department not found.");
}
