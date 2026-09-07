using Application.Common.Models;
using Application.Departments.Commands;
using Application.Departments.Commands.CreateDepartment;
using Application.Departments.Commands.DeleteDepartment;
using Application.Departments.Commands.UpdateDepartment;
using Application.Departments.Models;
using Application.Departments.Queries.GetDepartment;
using Application.Departments.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController(ISender sender, ILogger<DepartmentsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Response<List<DepartmentResponse>>>> Get(CancellationToken cancellationToken) =>
        Ok(Response<List<DepartmentResponse>>.Success(await sender.Send(new GetDepartmentsQuery(), cancellationToken)));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<DepartmentResponse>>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(Response<DepartmentResponse>.Success(await sender.Send(new GetDepartmentQuery(id), cancellationToken)));

    [HttpPost]
    public async Task<ActionResult<Response<DepartmentResponse>>> Create(DepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateDepartmentCommand(request), cancellationToken);
        logger.LogInformation("Department {Id} created", result.DepartmentId);
        return CreatedAtAction(nameof(GetById), new { id = result.DepartmentId }, Response<DepartmentResponse>.Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<DepartmentResponse>>> Update(int id, DepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateDepartmentCommand(id, request), cancellationToken);
        logger.LogInformation("Department {Id} updated", id);
        return Ok(Response<DepartmentResponse>.Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteDepartmentCommand(id), cancellationToken);
        logger.LogInformation("Department {Id} deleted", id);
        return NoContent();
    }
}
