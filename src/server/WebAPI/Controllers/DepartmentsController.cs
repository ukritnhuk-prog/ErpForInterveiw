using Application.Common.Models;
using Application.Departments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController(ISender sender, ILogger<DepartmentsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Response<List<DepartmentDto>>>> Get(CancellationToken cancellationToken) =>
        Ok(Response<List<DepartmentDto>>.Success(await sender.Send(new GetDepartmentsQuery(), cancellationToken)));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Response<DepartmentDto>>> GetById(int id, CancellationToken cancellationToken) =>
        Ok(Response<DepartmentDto>.Success(await sender.Send(new GetDepartmentQuery(id), cancellationToken)));

    [HttpPost]
    public async Task<ActionResult<Response<DepartmentDto>>> Create(DepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SaveDepartmentCommand(null, request), cancellationToken);
        logger.LogInformation("Department {Id} created", result.DepartmentId);
        return CreatedAtAction(nameof(GetById), new { id = result.DepartmentId }, Response<DepartmentDto>.Success(result));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<DepartmentDto>>> Update(int id, DepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SaveDepartmentCommand(id, request), cancellationToken);
        logger.LogInformation("Department {Id} updated", id);
        return Ok(Response<DepartmentDto>.Success(result));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteDepartmentCommand(id), cancellationToken);
        logger.LogInformation("Department {Id} deleted", id);
        return NoContent();
    }
}
