using Application.Common.Interfaces;
using Application.Departments.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Departments.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DepartmentResponse> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        DepartmentRequestValidator.Validate(request.Data);

        var department = new Department
        {
            DepartmentName = request.Data.DepartmentName.Trim(),
            DepartmentAddress = string.IsNullOrWhiteSpace(request.Data.DepartmentAddress)
                ? null
                : request.Data.DepartmentAddress.Trim()
        };

        _context.Departments.Add(department);
        await _context.SaveChangesAsync(cancellationToken);

        return await DepartmentProjection.ToResponse(
                _context.Departments.AsNoTracking().Where(item => item.DepartmentId == department.DepartmentId))
            .SingleAsync(cancellationToken);
    }
}
