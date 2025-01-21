using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Domain.Constants;
using Shifty.Persistence.Db;
using System;
using System.Linq;

namespace Shifty.Persistence.Services.MigrationManagers
{
    public static class MultipleDatabaseExtensions
    {
        public static async void AddAndMigrateTenantDatabases(this IServiceCollection services , IConfiguration configuration)
        {
            try
            {
                // Tenant Db Context (reference context) - get a list of tenants
                using var scopeTenant     = services.BuildServiceProvider().CreateScope();
                var       tenantDbContext = scopeTenant.ServiceProvider.GetRequiredService<TenantDbContext>();

                if ((await tenantDbContext.Database.GetPendingMigrationsAsync()).Any())
                    await tenantDbContext.Database.MigrateAsync(); // apply migrations on baseDbContext


                var tenantsInDb = await tenantDbContext.TenantInfo.ToListAsync();


                foreach (var tenant in tenantsInDb) // loop through all tenants, apply migrations on applicationDbContext
                {
                    var connectionString = string.IsNullOrEmpty(tenant.GetConnectionString())
                        ? ApplicationConstant.AppOptions.WriteDatabaseConnectionString
                        : tenant.GetConnectionString();

                    // Application Db Context (app - per tenant)
                    using var scopeApplication = services.BuildServiceProvider().CreateScope();
                    var       dbContext        = scopeApplication.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.SetConnectionString(connectionString);

                    await dbContext.Database.EnsureCreatedAsync();

                    if (!(await dbContext.Database.GetPendingMigrationsAsync()).Any())
                        continue;

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Applying Migrations for '{tenant.Id}' tenant.");
                    Console.ResetColor();
                    await dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}