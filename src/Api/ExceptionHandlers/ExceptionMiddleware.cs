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

// public class ExceptionMiddleware(
//     RequestDelegate next,
//     ILogger<ExceptionMiddleware> logger)
// {
//     public async Task InvokeAsync(HttpContext context)
//     {
//         try
//         {
//             await next(context);
//         }
//         catch (ValidationException ex)
//         {
//             context.Response.StatusCode = StatusCodes.Status400BadRequest;
//             context.Response.ContentType = "application/json";
//
//             var errors = ex.Errors.Select(e => new
//             {
//                 Field = e.PropertyName,
//                 Message = e.ErrorMessage
//             });
//
//             await context.Response.WriteAsync(JsonSerializer.Serialize(new
//             {
//                 Error = "Validation Failed",
//                 Details = errors
//             }));
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "Unhandled exception processing {Method} {Path}", context.Request.Method,
//                 context.Request.Path);
//             context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//             await context.Response.WriteAsync("An unexpected error occurred");
//         }
//     }
// }