using Application.Common.Models;
using Application.Employees;
using Application.Employees.Commands.CreateEmployee;
using Application.Employees.Commands.DeleteEmployee;
using Application.Employees.Commands.UpdateEmployee;
using Application.Employees.Queries.GetEmployee;
using Application.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController(ISender sender, ILogger<EmployeesController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Response<List<EmployeeDto>>>> Get([FromQuery] string? search, [FromQuery] int? departmentId, CancellationToken cancellationToken) =>
        Ok(Response<List<EmployeeDto>>.Success(await sender.Send(new GetEmployeesQuery(search, departmentId), cancellationToken)));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<EmployeeDto>>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(Response<EmployeeDto>.Success(await sender.Send(new GetEmployeeQuery(id), cancellationToken)));

    [HttpPost]
    public async Task<ActionResult<Response<EmployeeDto>>> Create(EmployeeRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateEmployeeCommand(request), cancellationToken);
        logger.LogInformation("Employee {Id} created", result.EmployeeId);
        return CreatedAtAction(nameof(GetById), new { id = result.EmployeeId }, Response<EmployeeDto>.Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<EmployeeDto>>> Update(int id, EmployeeRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateEmployeeCommand(id, request), cancellationToken);
        logger.LogInformation("Employee {Id} updated", id);
        return Ok(Response<EmployeeDto>.Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteEmployeeCommand(id), cancellationToken);
        logger.LogInformation("Employee {Id} deleted", id);
        return NoContent();
    }
}
