using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmartAttendance.ApiFramework.Tools;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.ApiFramework.Middleware.Jwt;

/// <summary>
///     Middleware that catches any exception thrown downstream (e.g., in JWT validation),
///     logs it, and returns a structured JSON problem‐details response.
/// </summary>
public class JwtExceptionHandlingMiddleware
{
    // Reuse a single JsonSerializerOptions to avoid re‐allocating on each exception.
    private readonly static JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        // Not indenting improves throughput (no need for pretty‐printed output).
        WriteIndented = false
    };

    private readonly ILogger<JwtExceptionHandlingMiddleware> _logger;

    private readonly RequestDelegate _next;

    /// <summary>
    ///     Constructor receives the next middleware in the pipeline and a logger.
    /// </summary>
    public JwtExceptionHandlingMiddleware(RequestDelegate next, ILogger<JwtExceptionHandlingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    /// <summary>
    ///     Invokes the next piece in the pipeline and catches any exception.
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Proceed to the next middleware/handler
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception (stack trace, message) once
            _logger.LogError(ex, "Unhandled exception occurred while processing the request.");

            // Delegate to a static helper to write a JSON problem‐detail response
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    ///     Writes a JSON payload to the response based on exception type.
    /// </summary>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;

        // Always return JSON
        response.ContentType = "application/json";

        // Distinguish between "authentication" errors (401) and all others (500).
        // This saves us from creating multiple ApiProblemDetails objects.
        var isAuthError =
            exception is AuthenticationFailureException or UnauthorizedAccessException or ForbiddenException;

        var statusCode = isAuthError
            ? HttpStatusCode.Unauthorized         // 401 for auth‐related exceptions
            : HttpStatusCode.InternalServerError; // 500 otherwise

        // Set the numeric status code
        response.StatusCode = (int)statusCode;

        // Choose a title that reflects the type of error
        var title = isAuthError
            ? "Authentication failed. Invalid or expired token."
            : "An unexpected error occurred.";

        // Populate the problem‐details object only once:
        var errorDetails = new ApiProblemDetails
        {
            Status = (int)statusCode,
            Title  = title,
            Detail = exception.Message
        };

        // Serialize using the shared JsonSerializerOptions to minimize per‐call overhead.
        var jsonPayload = JsonSerializer.Serialize(errorDetails, _jsonOptions);

        // Write the JSON directly to the response body and return the task.
        return response.WriteAsync(jsonPayload);
    }
}