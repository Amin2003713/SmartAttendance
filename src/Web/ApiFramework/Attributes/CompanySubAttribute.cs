using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace SmartAttendance.ApiFramework.Attributes;

public class CompanySubAttribute
    : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var services  = context.HttpContext.RequestServices;
        var localizer = services.GetRequiredService<IStringLocalizer<ProjectAccessAttribute>>();

        var mediator = services.GetService<IMediator>();


        await next();
    }
}