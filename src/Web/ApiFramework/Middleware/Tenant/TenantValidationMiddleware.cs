using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifty.ApiFramework.Tools;
using Shifty.Common;
using Shifty.Common.General;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Persistence.TenantServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shifty.ApiFramework.Middleware.Tenant
{
    public class TenantValidationMiddleware(RequestDelegate next , ITenantServiceExtension tenantService , IServiceProvider provider)
    {
        public async Task Invoke(HttpContext context , TenantDbContext tenantDbContext)
        {
            var appOption = provider.GetRequiredService<IAppOptions>();

            if (SkipTenantValidation(context))
            {
                await next(context);
                return;
            }

            tenantService.TenantContextAccessor = context.GetMultiTenantContext<ShiftyTenantInfo>();

            if (tenantService.GetId() == null)
            {
                var problemDetails = new ApiProblemDetails
                {
                    Status   = StatusCodes.Status400BadRequest , Title = "Tenant params validation failed"
                    , Detail = "Tenant parameters were not present in headers." , Errors = new Dictionary<string , List<string>>
                    {
                        {
                            "tenant params" , new List<string>
                            {
                                "Tenant parameters were not present in headers." ,
                            }
                        }
#if DEBUG
                        ,
                        {
                            "tip" , new List<string>
                            {
                                "Add the tenant identifier A.K.A domain to header like __tenant__:Domain" ,
                            }
                        }
                        ,
#endif
                    }
                    ,
                };

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
                    Status = StatusCodes.Status404NotFound , Title = "Tenant not found" , Detail = "The specified tenant was not found." ,
                };

                context.Response.StatusCode  = problemDetails.Status;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(problemDetails);
                return;
            }

            if (appOption != null)
                appOption.ReadDatabaseConnectionString = appOption.WriteDatabaseConnectionString = tenant.GetConnectionString();

            if (context.Request.Path.Value!.Contains("/api/") && !context.Request.Path.Value.Contains("/Panel/"))
            {
                if (!context.Request.Headers.TryGetValue("X_Device_Type" , out var deviceType))
                {
                    var problemDetails = new ApiProblemDetails
                    {
                        Status   = StatusCodes.Status400BadRequest , Title = "Tenant params validation failed"
                        , Detail = "Device type parameter was not present in headers." , Errors = new Dictionary<string , List<string>>
                        {
                            {
                                "tenant params" , [
                                    "Device type parameter was not present in headers." ,
                                ]
                            }
#if DEBUG
                            ,
                            {
                                "tip" , [
                                    "Add header like X_Device_Type:Device_Type => Browser, Android, iOS, Windows, macOS" ,
                                ]
                            }
                            ,
#endif
                        }
                        ,
                    };

                    context.Response.StatusCode  = problemDetails.Status;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(problemDetails);
                    return;
                }

                if (deviceType != "Browser")
                {
                    if (!context.Request.Headers.TryGetValue("__Hardware__" , out var hardwareId))
                    {
                        var problemDetails = new ApiProblemDetails
                        {
                            Status   = StatusCodes.Status400BadRequest , Title = "Tenant params validation failed"
                            , Detail = "Hardware ID parameter was not present in headers." , Errors = new Dictionary<string , List<string>>
                            {
                                {
                                    "tenant params" , new List<string>
                                    {
                                        "Hardware ID parameter was not present in headers." ,
                                    }
                                }
#if DEBUG
                                ,
                                {
                                    "tip" , [
                                        "Add header like __Hardware__:HardwareId if the request came from applications" ,
                                    ]
                                }
                                ,
#endif
                            }
                            ,
                        };

                        context.Response.StatusCode  = problemDetails.Status;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(problemDetails);
                        return;
                    }

                    await using var tenantDb   = new ReadOnlyDbContext(CreateContextOptions(tenantService.GetConnectionString()));
                    var             userExists = await tenantDb.Users.SingleOrDefaultAsync(u => u.HardwareId == hardwareId);

                    if (userExists.HardwareId == null)
                    {
                        if (await tenantDb.Users.AnyAsync(a => a.HardwareId == hardwareId))
                        {
                            var problemDetails = new ApiProblemDetails
                            {
                                Status   = (int)ApiResultStatusCode.UnAuthorized , Title = "Unauthorized"
                                , Detail = "This user name has already been registered by another device." ,
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
                            Status   = (int)ApiResultStatusCode.UnAuthorized , Title = "Unauthorized"
                            , Detail = "The provided hardware ID must match the user's device first login." ,
                        };

                        context.Response.StatusCode  = problemDetails.Status;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(problemDetails);
                        return;
                    }
                }
            }

            // Continue processing the request
            await next(context);
        }

        private static bool SkipTenantValidation(HttpContext context) =>
            context.Request.Path.Value!.EndsWith(".json") || context.Request.Path.Value!.EndsWith(".html") || context.Request.Path.Value!.EndsWith(".js") || context.Request.Path.Value!.EndsWith(".ts") || context.Request.Path.Value.Contains("/panel/");

        private static DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
            return contextOptions;
        }
    }
}