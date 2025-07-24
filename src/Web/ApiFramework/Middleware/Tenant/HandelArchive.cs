using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Shifty.Common.InjectionHelpers;

namespace Shifty.ApiFramework.Middleware.Tenant;

public class HandelArchive(
    RequestDelegate next,
    IMediator mediator,
    IStringLocalizer<HandelArchive> localizer
)
    : ITransientDependency
{
    public async Task Invoke(HttpContext context)
    {
        if (SkipTenantValidation(context))
        {
            await next(context);
            return;
        }
//
//         var tenant = await mediator.Send(new GetSettingQuery());
//
//
//         if (tenant == null)
//         {
//             var problemDetails = new ApiProblemDetails
//             {
//                 StatusBroker = 400 , Title = localizer["Validation Error"].Value , // "خطای اعتبارسنجی"
//                 Detail = localizer["Tenant parameters are missing."].Value , // "پارامترهای مستأجر وجود ندارد."
//                 Errors = new Dictionary<string , List<string>>
//                 {
//                     {
//                         "params" , [localizer["Tenant parameters are missing."].Value]
//                     }
// #if DEBUG
//                     ,
//                     {
//                         "tip" , [localizer["Ensure that the tenant ID is provided in the request."].Value]
//                     } // "اطمینان حاصل کنید که شناسه مستأجر در درخواست ارائه شده است."
// #endif
//                 }
//             };
//             Console.WriteLine("Header Exception");
//             context.Response.StatusCode  = problemDetails.StatusBroker;
//             context.Response.ContentType = "application/json";
//             await context.Response.WriteAsJsonAsync(problemDetails);
//             return;
//         }
//
//         if (!tenant.Flags.HasFlag(SettingFlags.CompanyEnabled))
//         {
//             if()
//         }

        await next(context);
    }

    private static bool SkipTenantValidation(HttpContext context)
    {
        return context.Request.Path.Value!.Contains("/api") &&
               (context.Request.Path.Value!.Contains("company") ||
                context.Request.Path.Value!.Contains("user") ||
                context.Request.Path.Value!.Contains("setting") ||
                context.Request.Path.Value!.Contains("payment"));
    }
}