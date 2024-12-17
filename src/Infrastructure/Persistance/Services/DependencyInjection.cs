using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Common.General;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Db;
using Shifty.Persistence.TenantServices;
using System.Reflection;

namespace Shifty.Persistence.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
            services.AddScoped<IPasswordHasher<TenantAdmin>, PasswordHasher<TenantAdmin>>();

            services.AddScoped<ITenantService , TenantServiceImplementation>();
            services.AddScoped<TenantMigrationService>();


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(appOptions.TenantStore));

            services.AddScoped<ReadOnlyDbContext>();
            services.AddScoped<WriteOnlyDbContext>();

            services.AddScoped((serviceProvider) =>
                                  {

                                      var tenantService = serviceProvider.GetRequiredService<ITenantService>();
                                      return tenantService != null ? new ReadOnlyDbContext(CreateContextOptions(tenantService.GetConnectionString()) , tenantService) : default!;
                                  });

            services.AddScoped((serviceProvider) =>
                                  {
                                      var tenantService = serviceProvider.GetRequiredService<ITenantService>();
                                      return tenantService != null ? new WriteOnlyDbContext(CreateContextOptions(tenantService.GetConnectionString()) , tenantService) : default!;
                                  });

            services.AddScoped<IAppDbContext, AppDbContext>(((serviceProvider) =>
                                                                {
                                                                    var tenantService = serviceProvider.GetRequiredService<ITenantService>();
                                                                    return new AppDbContext(CreateContextOptions(tenantService.GetConnectionString()), tenantService);
                                                                }));

            return services;

            DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
            {
                var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                     .UseSqlServer(connectionString)
                                     .Options;

                return contextOptions;
            }
        }
    }
}
