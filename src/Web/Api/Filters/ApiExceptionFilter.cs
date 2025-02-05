using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Resources.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Shifty.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        private readonly ILogger<ApiExceptionFilter>                                     _logger;
        private readonly CommonMessages                              _messages;
        

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger , CommonMessages messages)
        {
            _logger        = logger;
            _messages = messages;
            _exceptionHandlers = new Dictionary<Type , Action<ExceptionContext>>
            {
                {
                    typeof(ValidationException) , HandleValidationException
                } ,
                {
                    typeof(FluentValidation.ValidationException) , HandleFluentValidationException
                } ,
                {
                    typeof(NotFoundException) , HandleNotFoundException
                } ,
                {
                    typeof(ConflictException) , HandleConflictException
                } ,
                {
                    typeof(UnauthorizedAccessException) , HandleUnauthorizedException
                } ,
                {
                    typeof(ForbiddenException) , HandleForbiddenException
                },
                {
                    typeof(ShiftyException) , HandleShiftyException
                }
            };
        }

        private void HandleShiftyException(ExceptionContext context)
        {
            if (context.Exception is ShiftyException exception)
            {
                var problemDetails = new ApiProblemDetails
                {
                    Status = (int)exception.HttpStatusCode ,
                    Title  = exception.Message , // Localized title
                    Detail = exception.AdditionalData , // Localized detail
                    Errors = []
                };
                context.Result = exception.HttpStatusCode switch
                {

                    HttpStatusCode.Unauthorized or HttpStatusCode.NonAuthoritativeInformation => new UnauthorizedObjectResult(problemDetails) ,
                    HttpStatusCode.Forbidden or HttpStatusCode.BadRequest => new BadRequestObjectResult(problemDetails) ,
                    
                    HttpStatusCode.NotFound => new NotFoundObjectResult(problemDetails) ,
                    HttpStatusCode.Conflict => new ConflictObjectResult(problemDetails) ,
                    _ => new ObjectResult(problemDetails) ,
                };
                context.ExceptionHandled = true;
            }
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
                Status = StatusCodes.Status500InternalServerError, Title = _messages.InternalServerError_Title() , // Localized title
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
                    Status = StatusCodes.Status400BadRequest, Title = _messages.Validation_Title() , // Localized title
                    Detail = _messages.Validation_Detail() ,                                         // Localized detail
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
                    Status = StatusCodes.Status400BadRequest, Title = _messages.Validation_Title() , // Localized title
                    Detail = _messages.Validation_Detail() ,   
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
                Status = StatusCodes.Status404NotFound, Title = _messages.NotFound_Title() , // Localized title
                Detail = exception?.Message ?? _messages.NotFound_Detail()                   // Localized detail
            };

            context.Result = new NotFoundObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }

        private void HandleConflictException(ExceptionContext context)
        {
            var exception = context.Exception as ConflictException;
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status409Conflict, Title = _messages.Conflict_Title() , // Localized title
                Detail = exception?.Message ?? _messages.Conflict_Detail()                   // Localized detail
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status409Conflict,
            };

            context.ExceptionHandled = true;
        }

        private void HandleUnauthorizedException(ExceptionContext context)
        {
            var exception = context.Exception as ShiftyException ;
            _logger.LogCritical(exception.Message, exception.StackTrace);

            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized , 
                Title = _messages.Unauthorized_Title() , // Localized title
                Detail = _messages.Unauthorized_Detail()                                              // Localized detail
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status401Unauthorized ,
            };

            context.ExceptionHandled = true;
        }

        private void HandleForbiddenException(ExceptionContext context)
        {
            var exception = context.Exception as ForbiddenException;
 
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status403Forbidden ,
                Title  =  exception?.Source ??  _messages.Forbidden_Title() , // Localized title
                Detail = exception?.Message  ??  _messages.Forbidden_Detail()   // Localized detail
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status403Forbidden ,
            };

            context.ExceptionHandled = true;
        }


    }
}
