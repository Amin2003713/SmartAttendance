using Shifty.ApiFramework.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Shifty.ApiFramework.Attributes
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var modelStateEntries = context.ModelState.Values;

            foreach (var error in modelStateEntries.SelectMany(item => item.Errors))
                context.Result = new BadRequestObjectResult(error.ErrorMessage);
        }
    }
}
