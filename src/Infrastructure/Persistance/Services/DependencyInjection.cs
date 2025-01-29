using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Persistence.Services.MigrationManagers;
using System.Reflection;
using Shifty.Common.General;

namespace Shifty.Persistence.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher<TenantAdmin> , PasswordHasher<TenantAdmin>>();
            services.AddKeyedScoped<UserManager<TenantAdmin>>("Admins");


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(ApplicationConstant.AppOptions.TenantStore));


            services.AddScoped(provider =>
                               {
                                   var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                                   var tenantId            = httpContextAccessor?.HttpContext?.GetMultiTenantContext<ShiftyTenantInfo>();

                                   var options = CreateContextOptions(tenantId?.TenantInfo?.GetConnectionString() ??
                                                                      ApplicationConstant.AppOptions.ReadDatabaseConnectionString); // Implement this
                                   return new WriteOnlyDbContext(options);
                               });
            services.AddScoped(provider =>
                               {
                                   var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                                   var tenantId            = httpContextAccessor?.HttpContext?.GetMultiTenantContext<ShiftyTenantInfo>();

                                   var options = CreateContextOptions(tenantId?.TenantInfo?.GetConnectionString() ??
                                                                      ApplicationConstant.AppOptions.ReadDatabaseConnectionString); // Implement this
                                   return new ReadOnlyDbContext(options);
                               });

            services.AddScoped(provider =>
                               {
                                   var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                                   var tenantId            = httpContextAccessor?.HttpContext?.GetMultiTenantContext<ShiftyTenantInfo>();

                                   var options = CreateContextOptions(tenantId?.TenantInfo?.GetConnectionString() ??
                                                                      ApplicationConstant.AppOptions.ReadDatabaseConnectionString); // Implement this
                                   return new AppDbContext(options);
                               });

            services.AddScoped<IAppDbContext , AppDbContext>();

            services.AddAndMigrateTenantDatabases(configuration);
            services.AddScoped<RunTimeDatabaseMigrationService>();
            services.AddScoped<Seeder.Seeder>();
            return services;
        }

        private static DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
            return contextOptions;
        }
    }
}