using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Common.General;
using Shifty.Persistence.Db;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shifty.Persistence.Services.MigrationManagers
{
    public class RunTimeDatabaseMigrationService(IServiceProvider services, IConfiguration configuration , Seeder.Seeder seeder)
    {
        public async Task<bool> MigrateTenantDatabasesAsync(string tenantId)
        {
            try
            {
                // Tenant Db Context (reference context)
                using var scopeTenant     = services.CreateScope();
                var       tenantDbContext = scopeTenant.ServiceProvider.GetRequiredService<TenantDbContext>();

                if ((await tenantDbContext.Database.GetPendingMigrationsAsync()).Any())
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Applying BaseDb Migrations.");
                    Console.ResetColor();
                    await tenantDbContext.Database.MigrateAsync(); // apply migrations on baseDbContext
                }

                var tenant = await tenantDbContext.TenantInfo.FirstOrDefaultAsync(a=>a.Id == tenantId);
                var appOptions  = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

                
                    var connectionString = string.IsNullOrEmpty(tenant.GetConnectionString())
                        ? appOptions.WriteDatabaseConnectionString
                        : tenant.GetConnectionString();

                    // Application Db Context (app - per tenant)
                    using var scopeApplication = services.CreateScope();
                    var       dbContext        = scopeApplication.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.SetConnectionString(connectionString);

                    // await dbContext.Database.EnsureCreatedAsync();

                    if (!(await dbContext.Database.GetPendingMigrationsAsync()).Any())
                        return true;

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Applying Migrations for '{tenant.Id}' tenant.");
                    Console.ResetColor();
                    await dbContext.Database.MigrateAsync();

                    await seeder.Seed();

                return true;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"MigrationManagers Error: {e.Message}");
                Console.ResetColor();
                return false;
            }
        }
    }
}