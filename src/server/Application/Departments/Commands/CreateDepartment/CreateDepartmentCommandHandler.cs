using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department
        {
            DepartmentName = request.Data.DepartmentName.Trim(),
            DepartmentAddress = string.IsNullOrWhiteSpace(request.Data.DepartmentAddress)
                ? null
                : request.Data.DepartmentAddress.Trim()
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync(cancellationToken);

        return await DepartmentProjection.ToDto(
                _context.Departments.AsNoTracking().Where(item => item.DepartmentId == department.DepartmentId))
            .SingleAsync(cancellationToken);
    }
}
