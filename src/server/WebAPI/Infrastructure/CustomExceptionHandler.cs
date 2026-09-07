using Application.Common.Exceptions;
using Application.Common.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Infrastructure;

public sealed class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var (status, message) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            ValidationException => (StatusCodes.Status400BadRequest, exception.Message),
            DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "This record changed or was deleted. Refresh and try again."),
            DbUpdateException { InnerException: SqlException { Number: 547 } } =>
                (StatusCodes.Status400BadRequest, "The change conflicts with a department relationship or a data constraint. Refresh and check your values."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again.")
        };
        if (status == 500)
            // Do not log exception payloads or database values in this demo.
            logger.LogError("Unhandled {ExceptionType}. Trace: {TraceId}", exception.GetType().Name, context.TraceIdentifier);
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(Response<object>.Failure(message), cancellationToken);
        return true;
    }
}
