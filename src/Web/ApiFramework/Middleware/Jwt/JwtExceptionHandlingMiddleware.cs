using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Shifty.ApiFramework.Middleware.Jwt;

public class JwtExceptionHandlingMiddleware
{
    private readonly RequestDelegate                         _next;
    private readonly ILogger<JwtExceptionHandlingMiddleware> _logger;

    public JwtExceptionHandlingMiddleware(RequestDelegate next, ILogger<JwtExceptionHandlingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred while processing the request.");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorDetails = new
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message    = "An unexpected error occurred.",
            Details    = exception.Message
        };

        if (exception is AuthenticationFailureException or UnauthorizedAccessException)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            errorDetails = new
            {
                StatusCode = response.StatusCode,
                Message    = "Authentication failed. Invalid or expired token.",
                Details    = exception.Message
            };
        }

        var result = JsonSerializer.Serialize(errorDetails);
        return response.WriteAsync(result);
    }
}