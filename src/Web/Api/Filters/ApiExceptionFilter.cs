using Shifty.ApiFramework.Tools;
using Shifty.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shifty.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {

        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilter()
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(ExistingRecordException), HandleExistingRecordException },
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
            context.Result = new ApiResult<int>(-1, StatusCodes.Status500InternalServerError, new string[] { "an error occurred", context.Exception.Message });

            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            if (context.Exception is ValidationException exception)
            {
                // Flatten the validation errors into a list of strings
                var errorList = exception.Errors.Values.SelectMany(errors => errors).ToList();

                // Set the result to a BadRequestObjectResult
                context.Result = new BadRequestObjectResult(new
                {
                    Message = "Validation failed" , Errors = errorList ,
                });

                context.ExceptionHandled = true;
            }
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            context.Result = new ApiResult<int>(-1, StatusCodes.Status404NotFound, new string[] { "your resource not found", exception.Message });

            context.ExceptionHandled = true;
        }
        private void HandleExistingRecordException(ExceptionContext context)
        {
            var exception = context.Exception as ExistingRecordException;

            context.Result = new ApiResult<int>(-1, StatusCodes.Status500InternalServerError, new string[] { exception.Message });

            context.ExceptionHandled = true;
        }
    }
}