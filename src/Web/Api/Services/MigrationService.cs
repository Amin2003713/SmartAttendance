using Shifty.Persistence.Db;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Shifty.Api
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public interface IMigrationService
    {
        Task ApplyMigrations();
    }

    public class MigrationService(IServiceProvider serviceProvider) : IMigrationService
    {
        public async Task ApplyMigrations()
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Apply Migration
            try
            {
                if((await context.Database.GetPendingMigrationsAsync()).Any())
                   await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                // Log the error or handle it in some way
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
