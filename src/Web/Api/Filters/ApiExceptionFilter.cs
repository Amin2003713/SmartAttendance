using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shifty.ApiFramework.Tools;
using Shifty.Common.Exceptions;
using System;
using System.Collections.Generic;

namespace Shifty.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type , Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilter()
        {
            _exceptionHandlers = new Dictionary<Type , Action<ExceptionContext>>
            {
                {
                    typeof(ValidationException) , HandleValidationException
                }
                ,
                {
                    typeof(NotFoundException) , HandleNotFoundException
                }
                ,
                {
                    typeof(ExistingRecordException) , HandleExistingRecordException
                }
                ,
            };
        }


        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_exceptionHandlers.TryGetValue(type , out var handler))
            {
                handler.Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError , Title = "Internal Server Error" , Detail = context.Exception.Message ,
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError ,
            };

            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                var problemDetails = new ApiProblemDetails
                {
                    Status   = StatusCodes.Status400BadRequest , Title = "Validation failed" , Detail = "One or more validation errors occurred."
                    , Errors = exception.Errors ,
                };

                context.Result           = new BadRequestObjectResult(problemDetails);
                context.ExceptionHandled = true;
            }
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status404NotFound , Title = "Not Found" , Detail = exception?.Message ?? "The specified resource was not found." ,
            };

            context.Result           = new NotFoundObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }

        private void HandleExistingRecordException(ExceptionContext context)
        {
            var exception = context.Exception as ExistingRecordException;
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status409Conflict , Title = "Conflict" , Detail = exception?.Message ?? "An existing record conflict occurred." ,
            };

            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status409Conflict ,
            };

            context.ExceptionHandled = true;
        }
    }
}