using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Persistence.TenantServices;
using System.Threading.Tasks;

namespace Shifty.ApiFramework.TenentServices
{
    public class TenantValidationMiddleware(RequestDelegate next , ITenantService tenantService)
    {
        public async Task Invoke(HttpContext context, TenantDbContext tenantDbContext)
        {
            if (!context.Request.Path.Value!.Contains("/api/") || context.Request.Path.Value.EndsWith("Admins/sign-up"))
            {
                // If not, skip tenant validation and pass the request to the next middleware
                await next(context);
                return;
            }

            if(context.GetMultiTenantContext<ShiftyTenantInfo>().TenantInfo is not null)
                tenantService._httpContextAccessor = context.GetMultiTenantContext<ShiftyTenantInfo>();
            

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

            if (!context.Request.Path.Value!.EndsWith("/Login") || !context.Request.Path.Value!.EndsWith("Admins/sign-up"))
            {
                if (!context.Request.Headers.TryGetValue("X-User-Name", out var userName))
                {
                    context.Response.StatusCode = 400; // Bad Request
                    await context.Response.WriteAsync("User Name is missing.");
                    return;
                }

                // Validate if the user exists in the tenant's database
                await using var tenantDb   = new ReadOnlyDbContext(CreateContextOptions(tenantService.GetConnectionString()) , tenantService);
                var userExists = await tenantDb.Users.AnyAsync(u => u.Id == userName);
                
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