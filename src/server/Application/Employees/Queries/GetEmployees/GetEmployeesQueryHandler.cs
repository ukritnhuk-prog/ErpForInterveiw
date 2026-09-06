using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Employees.Queries.GetEmployees;

public sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Employee> query = _context.Employees.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim();
            query = query.Where(employee =>
                employee.FirstName.Contains(term) ||
                employee.LastName.Contains(term) ||
                (employee.FirstName + " " + employee.LastName).Contains(term) ||
                employee.Department.DepartmentName.Contains(term));
        }

        if (request.DepartmentId.HasValue)
            query = query.Where(employee => employee.DepartmentId == request.DepartmentId.Value);

        query = query
            .OrderBy(employee => employee.FirstName)
            .ThenBy(employee => employee.LastName)
            .ThenBy(employee => employee.EmployeeId);

        return EmployeeProjection.ToDto(query).ToListAsync(cancellationToken);
    }
}
