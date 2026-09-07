using Application.Common.Interfaces;
using Application.Departments.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Commands.UpdateDepartment;

public sealed class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DepartmentResponse> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        DepartmentRequestValidator.Validate(request.Data);

        var department = await _context.Departments
            .SingleOrDefaultAsync(item => item.DepartmentId == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Department not found.");

        department.DepartmentName = request.Data.DepartmentName.Trim();
        department.DepartmentAddress = string.IsNullOrWhiteSpace(request.Data.DepartmentAddress)
            ? null
            : request.Data.DepartmentAddress.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return await DepartmentProjection.ToResponse(
                _context.Departments.AsNoTracking().Where(item => item.DepartmentId == department.DepartmentId))
            .SingleAsync(cancellationToken);
    }
}
