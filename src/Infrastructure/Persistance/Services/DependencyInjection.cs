using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Common.General;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using Shifty.Persistence.Services.MigrationManagers;
using Shifty.Persistence.TenantServices;
using System;
using System.Reflection;

namespace Shifty.Persistence.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
            services.AddScoped<IPasswordHasher<TenantAdmin>, PasswordHasher<TenantAdmin>>();

            // services.AddScoped<ITenantServiceExtension , TenantExtension>();

           services.AddKeyedScoped<UserManager<TenantAdmin>>("Admins");

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(appOptions.TenantStore));


            services.AddScoped<WriteOnlyDbContext>(provider =>
                                                     {
                                                         var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                                                         var appptions = provider.GetService<IAppOptions>();
                                                         var tenantId            = httpContextAccessor?.HttpContext?.GetMultiTenantContext<ShiftyTenantInfo>();

                                                         var options = CreateContextOptions(tenantId?.TenantInfo?.GetConnectionString() ?? appptions.ReadDatabaseConnectionString); // Implement this
                                                         return new WriteOnlyDbContext(options);
                                                     });
            services.AddScoped<ReadOnlyDbContext>(provider =>
                                                   {
                                                       var httpContextAccessor = provider.GetService<IHttpContextAccessor>();
                                                       var appptions           = provider.GetService<IAppOptions>();
                                                       var tenantId            = httpContextAccessor?.HttpContext?.GetMultiTenantContext<ShiftyTenantInfo>();

                                                       var options = CreateContextOptions(tenantId.TenantInfo.GetConnectionString() ??
                                                                                          appptions.ReadDatabaseConnectionString); // Implement this
                                                       return new ReadOnlyDbContext(options);
                                                   });
            
            services.AddScoped<IAppDbContext, AppDbContext>();

            services.AddAndMigrateTenantDatabases(configuration);
            services.AddScoped<RunTimeDatabaseMigrationService>();
            services.AddScoped<Seeder.Seeder>();
            Console.WriteLine(appOptions.ReadDatabaseConnectionString);
            return services;
        }

        private static DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
            return contextOptions;
        }
    }

}
