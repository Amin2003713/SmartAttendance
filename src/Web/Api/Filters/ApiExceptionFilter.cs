using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shifty.ApiFramework.Tools;
using Shifty.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shifty.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        private readonly ILogger _logger = new LoggerFactory().CreateLogger("ApiExceptionFilter");

        public ApiExceptionFilter()
        {

            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(FluentValidation.ValidationException), HandleFluentValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(ConflictException), HandleConflictException }
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
                // Log known exceptions at Warning level
                _logger.LogWarning(context.Exception, "Handled exception of type {ExceptionType}", exceptionType.Name);
                return;
            }

            // Handle unknown exceptions
            HandleUnknownException(context);
            // Log unknown exceptions at Error level
            _logger.LogError(context.Exception, "Unhandled exception of type {ExceptionType}", exceptionType.Name);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = context.Exception.Message
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
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
                    Title = "Validation failed",
                    Detail = "One or more validation errors occurred.",
                    Errors = (exception.Errors)
                };

                context.Result = new BadRequestObjectResult(problemDetails);
                context.ExceptionHandled = true;
            }
        }

        private static Dictionary<string, List<string>> Errors(IEnumerable<ValidationFailure> failures)
        {
            var failureGroups = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage);
            if (!failureGroups.Any())
                return new Dictionary<string, List<string>>();

            var errors = new Dictionary<string, List<string>>();
            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToList();

                errors.Add(propertyName, propertyFailures);
            }

            return errors;
        }

        private void HandleFluentValidationException(ExceptionContext context)
        {
            if (context.Exception is FluentValidation.ValidationException exception)
            {
                var problemDetails = new ApiProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Detail = "One or more validation errors occurred.",
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
                Title = "Not Found",
                Detail = exception?.Message ?? "The specified resource was not found.",
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
                Title = "Conflict",
                Detail = exception?.Message ?? "A conflict occurred with an existing record.",
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status409Conflict,
            };

            context.ExceptionHandled = true;
        }
    }
}
