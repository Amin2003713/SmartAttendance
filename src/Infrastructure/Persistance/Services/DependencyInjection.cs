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

            services.AddScoped<ITenantService , TenantServiceImplementation>();


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=localhost;Database=Shifty;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;"));
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddDbContext<TenantDbContext>(options => options.UseSqlServer(appOptions.TenantStore));
            services.AddDbContext<ReadOnlyDbContext>(options => options.UseSqlServer(appOptions.ReadDatabaseConnectionString));
            services.AddDbContext<WriteOnlyDbContext>(options => options.UseSqlServer(appOptions.WriteDatabaseConnectionString));
            services.AddAndMigrateTenantDatabases(configuration);
            services.AddScoped<DatabaseMigrationService>();
            services.AddScoped<Seeder.Seeder>();

            return services;
        }
    }
}
