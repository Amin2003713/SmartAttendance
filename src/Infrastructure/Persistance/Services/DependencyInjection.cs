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


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(appOptions.TenantStore));

            services.AddScoped((serviceProvider) =>
                                  {

                                      var tenantInfo = serviceProvider.GetRequiredService<ShiftyTenantInfo>();
                                      var tenantService = serviceProvider.GetRequiredService<ITenantService>();
                                      return tenantInfo != null ? new ReadOnlyDbContext(CreateContextOptions(tenantInfo.GetConnectionString()) , tenantService) : null!;
                                  });

            services.AddScoped((serviceProvider) =>
                                  {

                                      var tenantService = serviceProvider.GetRequiredService<ITenantService>();
                                      var tenantInfo    = serviceProvider.GetRequiredService<ShiftyTenantInfo>();
                                      return tenantInfo != null ? new WriteOnlyDbContext(CreateContextOptions(tenantInfo.GetConnectionString()) , tenantService) : null!;
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
