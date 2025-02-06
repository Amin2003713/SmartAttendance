using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shifty.ApiFramework.Tools;
using Shifty.Common;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Persistence.TenantServices;
using Shifty.Resources.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.SystemConsole.Themes;
using Shifty.Common.Utilities;

namespace Shifty.ApiFramework.Middleware.Tenant
{
    public class TenantValidationMiddleware(RequestDelegate next , ITenantServiceExtension tenantService , CommonMessages messages , IServiceProvider services)   : ITransientDependency
    {
        public async Task Invoke(HttpContext context , TenantDbContext tenantDbContext)
        {
            foreach (var header in context.Request.Headers)
            {
                Console.WriteLine($"{header.Key}: {header.Value} ");
            }

            if (SkipTenantValidation(context))
            {
                await next(context);
                return;
            }

            tenantService.TenantContextAccessor = context.GetMultiTenantContext<ShiftyTenantInfo>();

            if (tenantService.GetId() == null )
            {
                var problemDetails = new ApiProblemDetails
                {
                    Status = 400,
                    Title = messages.Validation_Title_Generic() , Detail = messages.Tenant_Error_ParamsMissing() , Errors =
                        new Dictionary<string , List<string>>
                        {
                            {
                                "params" , [messages.Tenant_Error_ParamsMissing()]
                            }
#if DEBUG
                            ,
                            {
                                "tip" , [messages.Tenant_Tip_Params()]
                            }
#endif                                   
                        } ,
                };
                Console.WriteLine("header Exeption");
                context.Response.StatusCode  = problemDetails.Status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
                return;
            }

            var tenant = await tenantDbContext.TenantInfo.FirstOrDefaultAsync(t => t.Id == tenantService.GetId());

            if (tenant == null)
            {
                var problemDetails = new ApiProblemDetails
                {
                    Status = StatusCodes.Status404NotFound , Title = messages.Tenant_Error_NotFound() , Detail = messages.Tenant_Error_NotFound() ,
                };
                Console.WriteLine("tenant Null");

                context.Response.StatusCode  = problemDetails.Status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
                return;
            }


            if (context.Request.Path.Value!.Contains("/api/"))
            {
                if (!context.Request.Headers.TryGetValue("X-Device-Type" , out var deviceType))
                {
                    var problemDetails = new ApiProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest ,Title = messages.Validation_Title_Generic() , Detail = messages.Tenant_Error_ParamsMissing() , Errors = new Dictionary<string , List<string>>
                        {
                            {
                                "params" , [messages.Tenant_Error_ParamsMissing()]
                            }
#if DEBUG
                            ,
                            {
                                "tip" , [messages.Device_Tip_DeviceType()]
                            }

#endif
                        } ,
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

                if (!context.Request.Headers.TryGetValue("X-User-Name" , out var userName))
                {
                    var problemDetails = new ApiProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest , Title = messages.Validation_Title_Generic() , Detail = messages.Tenant_Error_ParamsMissing() ,
                        Errors = new Dictionary<string , List<string>>
                        {
                            {
                                "params" , [messages.Tenant_Error_ParamsMissing()]
                            }
#if DEBUG
                            ,
                            {
                                "tip" , [messages.User_Name_Missing_Tip()]
                            }

#endif
                        } ,
                    };

                    context.Response.StatusCode  = problemDetails.Status;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(problemDetails);
                    return;

                }

                if (!context.Request.Headers.TryGetValue("--Hardware--" , out var hardwareId))
                {
                    var problemDetails = new ApiProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest , Title = messages.Validation_Title_Generic() ,
                        Detail = messages.Tenant_Error_ParamsMissing() , Errors = new Dictionary<string , List<string>>
                        {
                            {
                                "params" , [messages.Tenant_Error_ParamsMissing()]
                            }
#if DEBUG
                            ,
                            {
                                "tip" , [messages.Device_Tip_Hardware()]
                            }
#endif
                        } ,
                    };

                    context.Response.StatusCode  = problemDetails.Status;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(problemDetails);
                    return;
                }

                using var serviceCollection = services.CreateScope();
                var       tenantDb          = serviceCollection.ServiceProvider.GetRequiredService<AppDbContext>();
                tenantDb.Database.SetConnectionString(tenantService.GetConnectionString());
                
                var             userExists = await tenantDb.Users.FirstOrDefaultAsync(u => u.UserName == userName[0]);

                if (userExists is null)
                {
                    Console.WriteLine($"12 the iuser with {userName[0]} dose not exist in {tenant.Identifier} with hardwareId {hardwareId[0]} and connecti  = {tenant.GetConnectionString()}");
                    var problemDetails = new ApiProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest , Title = messages.NotFound_Title() ,
                        Detail = messages.NotFound_Detail() ,
                    };

                    context.Response.StatusCode  = problemDetails.Status;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(problemDetails);
                    return;
                }


                if (userExists.HardwareId == null)
                {
                                        Console.WriteLine($"h is null the iuser with {userName[0]} dose not exist in {tenant.Identifier} with hardwareId {hardwareId[0]} ");

                    if (await tenantDb.Users.AnyAsync(a => a.HardwareId == hardwareId[0]))
                    {
                        var problemDetails = new ApiProblemDetails
                        {
                            Status = StatusCodes.Status401Unauthorized , Title = messages.Unauthorized_Access() ,
                            Detail = messages.Hardware_Error_AlreadyRegistered() ,
                        };

                        context.Response.StatusCode  = problemDetails.Status;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(problemDetails);
                        return;
                    }

                    userExists.HardwareId = hardwareId[0];
                    tenantDb.Users.Update(userExists);
                    await tenantDb.SaveChangesAsync();
                }

                if (userExists.HardwareId != hardwareId[0])
                {
                    var problemDetails = new ApiProblemDetails
                    {
                        Status = StatusCodes.Status401Unauthorized , Title = messages.Unauthorized_Access() ,
                        Detail = messages.Hardware_Error_Mismatch() ,
                    };

                    context.Response.StatusCode  = problemDetails.Status;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(problemDetails);
                    return;
                }
            }
            
            // Continue processing the request                                  
            await next(context);
        }

        private static bool SkipTenantValidation(HttpContext context)
        {
            return context.Request.Path.Value!.Contains("/scalar/v1")  ||context.Request.Path.Value!.EndsWith(".css") ||context.Request.Path.Value!.Contains("metrics")  || context.Request.Path.Value!.EndsWith(".json") ||context.Request.Path.Value!.EndsWith("swagger") ||
                   context.Request.Path.Value!.EndsWith(".html") || context.Request.Path.Value!.EndsWith(".js") || context.Request.Path.Value!.EndsWith(".ts") ||
                   context.Request.Path.Value.Contains("/panel/")||  context.Request.Path.Value.Contains("/health") ||  context.Request.Path.Value.Contains("favicon.ico") ;
        }

       
    }
}