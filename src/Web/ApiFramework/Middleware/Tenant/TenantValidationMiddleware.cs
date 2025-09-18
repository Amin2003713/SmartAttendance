using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SmartAttendance.ApiFramework.Tools;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Persistence.Db;

namespace SmartAttendance.ApiFramework.Middleware.Tenant;

public class TenantValidationMiddleware(
    RequestDelegate                              next,
    IStringLocalizer<TenantValidationMiddleware> localizer
)
    : ITransientDependency
{
    public async Task Invoke(HttpContext context, SmartAttendanceTenantDbContext ipaDbContext)
    {
        foreach (var header in context.Request.Headers)
        {
            Console.WriteLine($@"{header.Key}: {header.Value} ");
        }

        if (SkipTenantValidation(context))
        {
            await next(context);
            return;
        }

        var tenantService = context.GetMultiTenantContext<UniversityTenantInfo>().TenantInfo;

        if (tenantService is null || tenantService!.Id == null)
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = 400,
                Title  = localizer["Validation Error"].Value,               // "خطای اعتبارسنجی"
                Detail = localizer["Tenant parameters are missing."].Value, // "پارامترهای مستأجر وجود ندارد."
                Errors = new Dictionary<string, List<string>>
                {
                    {
                        "params", [localizer["Tenant parameters are missing."].Value]
                    }
#if DEBUG
                   ,
                    {
                        "tip", [localizer["Ensure that the tenant ID is provided in the request."].Value]
                    }, // "اطمینان حاصل کنید که شناسه مستأجر در درخواست ارائه شده است."
#endif
                }
            };

            Console.WriteLine(@"Header Exception");
            context.Response.StatusCode  = problemDetails.Status;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problemDetails);
            return;
        }

        var tenant = await ipaDbContext.TenantInfo.FirstOrDefaultAsync(t => t.Id == tenantService.Id);

        if (tenant == null)
        {
            var problemDetails = new ApiProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title  = localizer["Tenant not found."].Value,                   // "مستأجر یافت نشد."
                Detail = localizer["The requested tenant does not exist."].Value // "مستأجر درخواست‌شده وجود ندارد."
            };

            Console.WriteLine(@"Tenant Null");

            context.Response.StatusCode  = problemDetails.Status;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problemDetails);
            return;
        }

        if (context.Request.Path.Value!.Contains("/api/") &&
            !context.Request.Path.Value!.Contains("/api/payment/verify"))
        {
            if (!context.Request.Headers.TryGetValue("X-Device-Type", out var deviceType))
            {
                var problemDetails = new ApiProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title  = localizer["Validation Error"].Value,        // "خطای اعتبارسنجی"
                    Detail = localizer["Device type is missing."].Value, // "نوع دستگاه مشخص نشده است."
                    Errors = new Dictionary<string, List<string>>
                    {
                        {
                            "params", [localizer["Device type is missing."].Value]
                        }
#if DEBUG
                       ,
                        {
                            "tip", [localizer["Ensure that 'X-Device-Type' header is included in the request."].Value]
                        }, // "اطمینان حاصل کنید که هدر 'X-Device-Type' در درخواست گنجانده شده است."
#endif
                    }
                };

                context.Response.StatusCode  = problemDetails.Status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
                return;
            }

            if (deviceType == "Browser")
            {
                await next(context);
                return;
            }
        }

        // Continue processing the request
        await next(context);
    }


    private static bool SkipTenantValidation(HttpContext context)
    {
        return context.Request.Path.Value!.Contains("/scalar/v1")          ||
               context.Request.Path.Value!.EndsWith(".css")                ||
               context.Request.Path.Value!.Contains("metrics")             ||
               context.Request.Path.Value!.EndsWith(".json")               ||
               context.Request.Path.Value!.EndsWith("swagger")             ||
               context.Request.Path.Value!.Contains("/api/payment/verify") ||
               context.Request.Path.Value!.EndsWith(".html")               ||
               context.Request.Path.Value!.EndsWith(".js")                 ||
               context.Request.Path.Value!.EndsWith(".ts")                 ||
               context.Request.Path.Value.Contains("/panel/")              ||
               context.Request.Path.Value.Contains("/health")              ||
               context.Request.Path.Value.Contains("/api/minio/hub-file/") ||
               context.Request.Path.Value.Contains("/api/hub-file/")       ||
               context.Request.Path.Value.Contains("/api/auth/")           ||
               context.Request.Path.Value.Contains("/scalar")              ||
               context.Request.Path.Value.Contains("favicon.ico");
    }
}

//             if (!context.Requests.Headers.TryGetValue("X-User-Name" , out var userName))
//             {
//                 var problemDetails = new ApiProblemDetails
//                 {
//                     StatusBroker = StatusCodes.Status400BadRequest , Title = messages.Validation_Title_Generic() , Detail = messages.Tenant_Error_ParamsMissing() ,
//                     Errors = new Dictionary<string , List<string>>
//                     {
//                         {
//                             "params" , [messages.Tenant_Error_ParamsMissing()]
//                         }
// #if DEBUG
//                         ,
//                         {
//                             "tip" , [messages.User_Name_Missing_Tip()]
//                         } ,
//
// #endif
//                     } ,
//                 };
//
//                 context.Response.StatusCode  = problemDetails.StatusBroker;
//                 context.Response.ContentType = "application/json";
//                 await context.Response.WriteAsJsonAsync(problemDetails);
//                 return;
//             }
//
//             if (!context.Requests.Headers.TryGetValue("--Hardware--" , out var hardwareId))
//             {
//                 var problemDetails = new ApiProblemDetails
//                 {
//                     StatusBroker = StatusCodes.Status400BadRequest , Title = messages.Validation_Title_Generic() , Detail = messages.Tenant_Error_ParamsMissing() ,
//                     Errors = new Dictionary<string , List<string>>
//                     {
//                         {
//                             "params" , [messages.Tenant_Error_ParamsMissing()]
//                         }
// #if DEBUG
//                         ,
//                         {
//                             "tip" , [messages.Device_Tip_Hardware()]
//                         } ,
// #endif
//                     } ,
//                 };
//
//                 context.Response.StatusCode  = problemDetails.StatusBroker;
//                 context.Response.ContentType = "application/json";
//                 await context.Response.WriteAsJsonAsync(problemDetails);
//                 return;
//             }
//
//             using var serviceCollection = services.CreateScope();
//             var       tenantDb          = serviceCollection.ServiceProvider.GetRequiredService<AppDbContext>();
//             tenantDb.Database.SetConnectionString(tenantService.GetConnectionString());
//
//             var userExists = await tenantDb.Users.FirstOrDefaultAsync(u => u.UserName == userName[0]);
//
//             if (userExists is null)
//             {
//                 Console.WriteLine(
//                     $"12 the iuser with {userName[0]} dose not exist in {tenant.Identifier} with hardwareId {hardwareId[0]} and connecti  = {tenant.GetConnectionString()}");
//                 var problemDetails = new ApiProblemDetails
//                 {
//                     StatusBroker = StatusCodes.Status400BadRequest , Title = messages.NotFound_Title() , Detail = messages.NotFound_Detail() ,
//                 };
//
//                 context.Response.StatusCode  = problemDetails.StatusBroker;
//                 context.Response.ContentType = "application/json";
//                 await context.Response.WriteAsJsonAsync(problemDetails);
//                 return;
//             }
//
//
//             if (userExists.HardwareId == null)
//             {
//                 Console.WriteLine($"h is null the iuser with {userName[0]} dose not exist in {tenant.Identifier} with hardwareId {hardwareId[0]} ");
//
//                 if (await tenantDb.Users.AnyAsync(a => a.HardwareId == hardwareId[0]))
//                 {
//                     var problemDetails = new ApiProblemDetails
//                     {
//                         StatusBroker = StatusCodes.Status401Unauthorized , Title = messages.Unauthorized_Access() ,
//                         Detail = messages.Hardware_Error_AlreadyRegistered() ,
//                     };
//
//                     context.Response.StatusCode  = problemDetails.StatusBroker;
//                     context.Response.ContentType = "application/json";
//                     await context.Response.WriteAsJsonAsync(problemDetails);
//                     return;
//                 }
//
//                 userExists.HardwareId = hardwareId[0];
//                 tenantDb.Users.Update(userExists);
//                 await tenantDb.SaveChangesAsync();
//             }
//
//             if (userExists.HardwareId != hardwareId[0])
//             {
//                 var problemDetails = new ApiProblemDetails
//                 {
//                     StatusBroker = StatusCodes.Status401Unauthorized , Title = messages.Unauthorized_Access() , Detail = messages.Hardware_Error_Mismatch() ,
//                 };
//
//                 context.Response.StatusCode  = problemDetails.StatusBroker;
//                 context.Response.ContentType = "application/json";
//                 await context.Response.WriteAsJsonAsync(problemDetails);
//                 return;
//             }