using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.ExceptionHandlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        var (statusCode, title, extensions) = exception switch
        {
            ValidationException ex => (
                StatusCodes.Status400BadRequest,
                "Validation Failed",
                new Dictionary<string, object?>
                {
                    ["errors"] = ex.Errors.Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage })
                }),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", null)
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

        context.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Instance = context.Request.Path
        };

        if (extensions is not null)
            foreach (var (key, value) in extensions)
                problemDetails.Extensions[key] = value;

        await context.Response.WriteAsJsonAsync(problemDetails, ct);
        return true;
    }
}