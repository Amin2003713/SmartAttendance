using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Persistence.Db;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shifty.Persistence.Services.MigrationManagers
{
    public class MigrationService(IServiceProvider serviceProvider) : IMigrationService
    {
        public async Task ApplyMigrations()
        {
            using var scope   = serviceProvider.CreateScope();
            var       context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Apply MigrationManagers
            try
            {
                if ((await context.Database.GetPendingMigrationsAsync()).Any())
                    await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}