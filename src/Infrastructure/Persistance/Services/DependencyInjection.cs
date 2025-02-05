using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Persistence.Services.MigrationManagers;
using System.Reflection;
using Shifty.Common.General;
using Shifty.Common.Utilities;

namespace Shifty.Persistence.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<TenantAdmin> , PasswordHasher<TenantAdmin>>();
            services.AddKeyedScoped<UserManager<TenantAdmin>>("Admins");
            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(ApplicationConstant.AppOptions.TenantStore));
            services.AddScoped<IdentityService>();

            services.AddScoped(provider =>
                               {
                                   var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                                   var identity = provider.GetService<IdentityService>();
                                   var tenantId            = httpContextAccessor?.HttpContext?.GetMultiTenantContext<ShiftyTenantInfo>();

                                   var options = CreateContextOptions(tenantId?.TenantInfo?.GetConnectionString() ??
                                                                      ApplicationConstant.AppOptions.ReadDatabaseConnectionString); // Implement this
                                   return new WriteOnlyDbContext(options , identity.GetUserId());
                               });
            services.AddScoped(provider =>
                               {
                                   var identity = provider.GetService<IdentityService>();
                                   var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                                   var tenantId            = httpContextAccessor?.HttpContext?.GetMultiTenantContext<ShiftyTenantInfo>();

                                   var options = CreateContextOptions(tenantId?.TenantInfo?.GetConnectionString() ??
                                                                      ApplicationConstant.AppOptions.ReadDatabaseConnectionString); // Implement this
                                   return new ReadOnlyDbContext(options  , identity.GetUserId());
                               });

            services.AddScoped(provider =>
                               {
                                   var identity = provider.GetService<IdentityService>();
                                   var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                                   var tenantId            = httpContextAccessor?.HttpContext?.GetMultiTenantContext<ShiftyTenantInfo>();

                                   var options = CreateContextOptions(tenantId?.TenantInfo?.GetConnectionString() ??
                                                                      ApplicationConstant.AppOptions.ReadDatabaseConnectionString); // Implement this
                                   return new AppDbContext(options , identity.GetUserId());
                               });


            services.AddAndMigrateTenantDatabases();
            return services;
        }

        private static DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
            return contextOptions;
        }
    }
}