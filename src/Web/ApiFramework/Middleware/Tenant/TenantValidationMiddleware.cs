using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Common.General;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Persistence.TenantServices;
using System;
using System.Threading.Tasks;

namespace Shifty.ApiFramework.Middleware.Tenant
{
    public class TenantValidationMiddleware(RequestDelegate next , ITenantServiceExtension tenantService , IServiceProvider provider)
    {
        public async Task Invoke(HttpContext context, TenantDbContext tenantDbContext)
        {
            var appOption = provider.GetRequiredService<IAppOptions>();
            tenantService.TenantContextAccessor = context.GetMultiTenantContext<ShiftyTenantInfo>();

            if (!context.Request.Path.Value!.Contains("/api/") || context.Request.Path.Value.Contains("/AdminsPanel/"))
            {
                await next(context);
                return;
            }
            

            if (tenantService.GetId() is null)
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Tenant ID is missing in the route.");
                return;
            }

            if (string.IsNullOrEmpty(tenantService.GetIdentifier()))
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Tenant ID is missing.");
                return;
            }


            // Validate Tenant in TenantDbContext
            var tenant = await tenantDbContext.TenantInfo.FirstOrDefaultAsync(t => t.Id == tenantService.GetId());

           

            if (tenant == null)
            {
                context.Response.StatusCode = 404; // Not Found
                await context.Response.WriteAsync("Tenant not found.");
                return;
            }

            if (appOption is not null)
                appOption.ReadDatabaseConnectionString = appOption.WriteDatabaseConnectionString = tenant.GetConnectionString();

            if (context.Request.Path.Value!.Contains("/api/") || !context.Request.Path.Value.Contains("/AdminsPanel/"))
            {
                if (!context.Request.Headers.TryGetValue("X-User-Name", out var userName))
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("User Name is missing.");
                    return;
                }

                // Validate if the user exists in the tenant's database
                await using var tenantDb   = new ReadOnlyDbContext(CreateContextOptions(tenantService.GetConnectionString()));
                var userExists = await tenantDb.Users.AnyAsync(u => u.UserName == userName[0].ToString());
                
                if (!userExists)
                {
                    context.Response.StatusCode = 403; // Forbidden
                    await context.Response.WriteAsync("User does not exist in this tenant's database.");
                    return;
                }
            }

            // Continue processing the request
            await next(context);
        }

        private static DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
            return contextOptions;
        }
    }
}