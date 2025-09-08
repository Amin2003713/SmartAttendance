using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SmartAttendance.ApiFramework.Tools;

namespace SmartAttendance.ApiFramework.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Resolve a localizer for our attribute’s own messages
        var localizer = context.HttpContext.RequestServices
            .GetRequiredService<IStringLocalizer<ValidateModelStateAttribute>>();

        // Collect all validation failures from FluentValidation
        var allFailures = new Dictionary<string, List<string>>();

        foreach (var (argumentName, argumentValue) in context.ActionArguments)
        {
            if (argumentValue == null)
                continue;

            // Attempt to find an IValidator<T> for this argument’s runtime type
            var argumentType  = argumentValue.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            var validator     = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator == null)
                continue; // No validator registered for this type

            // Create a ValidationContext<object> so we can call ValidateAsync dynamically
            var validationContext = new ValidationContext<object>(argumentValue);
            var validationResult  = await validator.ValidateAsync(validationContext);

            if (validationResult.IsValid)
                continue;

            // Group failures by property name
            foreach (var failure in validationResult.Errors)
            {
                // If failure.PropertyName is empty, put under argumentName
                var key = string.IsNullOrWhiteSpace(failure.PropertyName)
                    ? argumentName
                    : $"{argumentName}.{failure.PropertyName}";

                if (!allFailures.ContainsKey(key))
                    allFailures[key] = new List<string>();

                allFailures[key].Add(failure.ErrorMessage);
            }
        }

        // If any failures, short-circuit and return 400
        if (allFailures.Any())
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = localizer["Validation Error"],
                Detail = localizer["Validation failed. Please check the entered data."].Value,
                Errors = allFailures
            };

            context.Result = new BadRequestObjectResult(problemDetails);
            return;
        }

        // If no FluentValidation failures, proceed to next filter/action
        await next();
    }
}