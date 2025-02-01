using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Persistence.Db;
using System;
using System.Linq;
using Shifty.Common.General;
using Shifty.Domain.Tenants;

namespace Shifty.Persistence.Services.MigrationManagers
{
    public static class MultipleDatabaseExtensions
    {
        public static async void AddAndMigrateTenantDatabases(this IServiceCollection services)
        {
            try
            {
                // Tenant Db Context (reference context) - get a list of tenants
                using var scopeTenant     = services.BuildServiceProvider().CreateScope();
                var       tenantDbContext = scopeTenant.ServiceProvider.GetRequiredService<TenantDbContext>();

                if ((await tenantDbContext.Database.GetPendingMigrationsAsync()).Any())
                    await tenantDbContext.Database.MigrateAsync(); // apply migrations on baseDbContext

                if (await tenantDbContext.TenantInfo.AnyAsync(a => a.Identifier == "grafana"))
                    await tenantDbContext.TenantInfo.AddAsync(new ShiftyTenantInfo()
                    {
                        Identifier = "grafana",
                        Name = "Grafana",
                    });

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

                    await dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}