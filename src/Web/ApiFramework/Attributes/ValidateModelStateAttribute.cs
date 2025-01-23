using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Shifty.ApiFramework.Tools;
using Shifty.Resources.Messages;
using System.Collections.Generic;
using System.Linq;

namespace Shifty.ApiFramework.Attributes
{
    public class ValidateModelStateAttribute() : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var messages = context.HttpContext.RequestServices.GetRequiredService<CommonMessages>();
            if (context.ModelState.IsValid)
                return;

            var errors = new Dictionary<string , List<string>>();

            foreach (var entry in context.ModelState)
            {
                var field       = entry.Key;
                var fieldErrors = entry.Value.Errors.Select(error => error.ErrorMessage).ToList();

                errors.Add(field , fieldErrors);
            }

            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status400BadRequest , Title = messages.Validation_Title_Generic() , Detail = messages.Validation_Detail() ,
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(problemDetails);
        }

    }
}