using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Departments;

public record GetDepartmentsQuery : IRequest<List<DepartmentDto>>;
public record GetDepartmentQuery(int Id) : IRequest<DepartmentDto>;
public record SaveDepartmentCommand(int? Id, DepartmentRequest Data) : IRequest<DepartmentDto>;
public record DeleteDepartmentCommand(int Id) : IRequest;

public sealed class DepartmentHandlers(IDepartmentRepository repository) :
    IRequestHandler<GetDepartmentsQuery, List<DepartmentDto>>,
    IRequestHandler<GetDepartmentQuery, DepartmentDto>,
    IRequestHandler<SaveDepartmentCommand, DepartmentDto>,
    IRequestHandler<DeleteDepartmentCommand>
{
    public Task<List<DepartmentDto>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken) =>
        repository.GetAsync(cancellationToken);

    public async Task<DepartmentDto> Handle(GetDepartmentQuery request, CancellationToken cancellationToken) =>
        await repository.GetByIdAsync(request.Id, cancellationToken)
        ?? throw new KeyNotFoundException("Department not found.");

    public async Task<DepartmentDto> Handle(SaveDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = request.Id.HasValue
            ? await repository.FindAsync(request.Id.Value, cancellationToken)
                ?? throw new KeyNotFoundException("Department not found.")
            : new Department();
        department.DepartmentName = request.Data.DepartmentName.Trim();
        department.DepartmentAddress = string.IsNullOrWhiteSpace(request.Data.DepartmentAddress)
            ? null : request.Data.DepartmentAddress.Trim();
        if (!request.Id.HasValue)
            repository.Add(department);
        await repository.SaveChangesAsync(cancellationToken);
        return (await repository.GetByIdAsync(department.DepartmentId, cancellationToken))!;
    }

    public async Task Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await repository.FindAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Department not found.");
        if (await repository.HasEmployeesAsync(request.Id, cancellationToken))
            throw new ValidationException("Cannot delete this department because employees are currently assigned to it.");
        repository.Remove(department);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
