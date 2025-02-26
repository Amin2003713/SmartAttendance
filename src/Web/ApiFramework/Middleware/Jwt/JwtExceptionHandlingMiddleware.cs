using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shifty.ApiFramework.Tools;
using Shifty.Resources.Messages;

namespace Shifty.ApiFramework.Middleware.Jwt;

public class JwtExceptionHandlingMiddleware
{
    private readonly RequestDelegate                         _next;
    private readonly ILogger<JwtExceptionHandlingMiddleware> _logger;
    private readonly CommonMessages _messages;

    public JwtExceptionHandlingMiddleware(RequestDelegate next, ILogger<JwtExceptionHandlingMiddleware> logger , CommonMessages messages)
    {
        _next          = next;
        _logger        = logger;
        _messages = messages;
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorDetails = new  ApiProblemDetails()
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title  = _messages.Server_Error() ,
            Detail = exception.Message
        };

        if (exception is AuthenticationFailureException or UnauthorizedAccessException)
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            errorDetails = new ApiProblemDetails()
            {
                Status = response.StatusCode,
                Title  = _messages.Unauthorized_Title() ,
                Detail = exception.Message ?? _messages.Unauthorized_Detail(),
            };
        }

        var result = JsonSerializer.Serialize(errorDetails);
        return response.WriteAsync(result);
    }
}