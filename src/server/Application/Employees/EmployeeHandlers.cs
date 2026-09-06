using System.ComponentModel.DataAnnotations;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Employees;

public record GetEmployeesQuery(string? Search, int? DepartmentId) : IRequest<List<EmployeeDto>>;
public record GetEmployeeQuery(int Id) : IRequest<EmployeeDto>;
public record SaveEmployeeCommand(int? Id, EmployeeRequest Data) : IRequest<EmployeeDto>;
public record DeleteEmployeeCommand(int Id) : IRequest;

public sealed class EmployeeHandlers(IEmployeeRepository repository, IDepartmentRepository departments) :
    IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>,
    IRequestHandler<GetEmployeeQuery, EmployeeDto>,
    IRequestHandler<SaveEmployeeCommand, EmployeeDto>,
    IRequestHandler<DeleteEmployeeCommand>
{
    public Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken) =>
        repository.GetAsync(request.Search, request.DepartmentId, cancellationToken);

    public async Task<EmployeeDto> Handle(GetEmployeeQuery request, CancellationToken cancellationToken) =>
        await repository.GetByIdAsync(request.Id, cancellationToken)
        ?? throw new KeyNotFoundException("Employee not found.");

    public async Task<EmployeeDto> Handle(SaveEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = request.Id.HasValue
            ? await repository.FindAsync(request.Id.Value, cancellationToken)
                ?? throw new KeyNotFoundException("Employee not found.")
            : new Employee();
        if (!await departments.ExistsAsync(request.Data.DepartmentId, cancellationToken))
            throw new ValidationException("The selected department does not exist.");
        employee.DepartmentId = request.Data.DepartmentId;
        employee.FirstName = request.Data.FirstName.Trim();
        employee.LastName = request.Data.LastName.Trim();
        employee.Gender = request.Data.Gender;
        employee.DateOfBirth = request.Data.DateOfBirth;
        employee.DateJoined = request.Data.DateJoined;
        employee.EmployeeAddress = string.IsNullOrWhiteSpace(request.Data.EmployeeAddress)
            ? null : request.Data.EmployeeAddress.Trim();
        if (!request.Id.HasValue)
            repository.Add(employee);
        await repository.SaveChangesAsync(cancellationToken);
        return (await repository.GetByIdAsync(employee.EmployeeId, cancellationToken))!;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await repository.FindAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Employee not found.");
        repository.Remove(employee);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
