using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SmartAttendance.ApiFramework.Tools;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.ApiFramework.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly IStringLocalizer<ApiExceptionFilter>        _localizer;
    private readonly ILogger<ApiExceptionFilter>                 _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IStringLocalizer<ApiExceptionFilter> localizer)
    {
        _logger = logger;
        _localizer = localizer;
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            {
                typeof(ValidationException), HandleValidationException
            },
            {
                typeof(FluentValidation.ValidationException), HandleFluentValidationException
            },
            {
                typeof(NotFoundException), HandleNotFoundException
            },
            {
                typeof(ConflictException), HandleConflictException
            },
            {
                typeof(UnauthorizedAccessException), HandleUnauthorizedException
            },
            {
                typeof(ForbiddenException), HandleForbiddenException
            },
            {
                typeof(SmartAttendanceException), HandleSmException
            }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var exceptionType = context.Exception.GetType();

        if (_exceptionHandlers.TryGetValue(exceptionType, out var handler))
        {
            handler.Invoke(context);
            _logger.LogWarning(context.Exception, "Handled exception of type {ExceptionType}", exceptionType.Name);
            return;
        }

        HandleUnknownException(context);
        _logger.LogError(context.Exception, "Unhandled exception of type {ExceptionType}", exceptionType.Name);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var problemDetails = new ApiProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = _localizer["Internal Server Error"].Value, // "خطای داخلی سرور"
            Detail = context.Exception.Message
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        if (context.Exception is ValidationException exception)
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = _localizer["Validation Error"].Value,    // "خطای اعتبارسنجی"
                Detail = _localizer["Validation details"].Value, // "جزئیات اعتبارسنجی"
                Errors = exception.Errors
            };

            context.Result = new BadRequestObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
    }

    private static Dictionary<string, List<string>> Errors(IEnumerable<ValidationFailure> failures)
    {
        var failureGroups = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage);
        return !failureGroups.Any()
            ? new Dictionary<string, List<string>>()
            : failureGroups.ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToList());
    }

    private void HandleFluentValidationException(ExceptionContext context)
    {
        if (context.Exception is FluentValidation.ValidationException exception)
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = _localizer["Validation Error"].Value,    // "خطای اعتبارسنجی"
                Detail = _localizer["Validation details"].Value, // "جزئیات اعتبارسنجی"
                Errors = Errors(exception.Errors)
            };

            context.Result = new BadRequestObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = context.Exception as NotFoundException;
        var problemDetails = new ApiProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = _localizer["Not Found"].Value, // "یافت نشد"
            Detail = exception?.Message ??
                     _localizer["The requested resource was not found."].Value // "منبع مورد نظر یافت نشد"
        };

        context.Result = new NotFoundObjectResult(problemDetails);
        context.ExceptionHandled = true;
    }

    private void HandleConflictException(ExceptionContext context)
    {
        var exception = context.Exception as ConflictException;
        var problemDetails = new ApiProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = _localizer["Conflict"].Value,                                 // "تداخل"
            Detail = exception?.Message ?? _localizer["Conflict occurred."].Value // "تداخل رخ داده است"
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status409Conflict
        };

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedException(ExceptionContext context)
    {
        var exception = context.Exception as UnauthorizedAccessException;
        _logger.LogCritical(exception!.Message, exception.StackTrace);

        var problemDetails = new ApiProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = _localizer["Unauthorized"].Value, // "غیرمجاز"
            Detail = _localizer["You are not authorized to perform this action."]
                .Value // "شما مجاز به انجام این عملیات نیستید"
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenException(ExceptionContext context)
    {
        var exception = context.Exception as ForbiddenException;

        var problemDetails = new ApiProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = _localizer["Forbidden"].Value, // "ممنوع"
            Detail = exception?.Message ??
                     _localizer["You do not have permission to access this resource."]
                         .Value // "شما اجازه دسترسی به این منبع را ندارید"
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        context.ExceptionHandled = true;
    }

    private void HandleSmException(ExceptionContext context)
    {
        if (context.Exception is not SmartAttendanceException exception)
            return;

        var problemDetails = new ApiProblemDetails
        {
            Status = (int)exception.HttpStatusCode,
            Title = exception.Message,                                                      // Localized title
            Detail = exception.AdditionalData ?? _localizer["Internal Server Error"].Value, // Localized detail
            Errors = []
        };

        context.Result = exception.HttpStatusCode switch
                         {
                             HttpStatusCode.Unauthorized or HttpStatusCode.NonAuthoritativeInformation => new UnauthorizedObjectResult(
                                 problemDetails),
                             HttpStatusCode.Forbidden or HttpStatusCode.BadRequest => new BadRequestObjectResult(problemDetails),
                             HttpStatusCode.NotFound => new NotFoundObjectResult(problemDetails),
                             HttpStatusCode.Conflict => new ConflictObjectResult(problemDetails), _ => new ObjectResult(problemDetails)
                             {
                                 StatusCode = (int)exception.HttpStatusCode
                             }
                         };


        context.ExceptionHandled = true;
    }
}