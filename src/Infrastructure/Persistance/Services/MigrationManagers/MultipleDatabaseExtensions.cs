using SmartAttendance.Application.Interfaces.Base;

namespace SmartAttendance.Persistence.Services.MigrationManagers;

public static class MultipleDatabaseExtensions
{
    /// <summary>
    ///     Migrates all tenant databases and seeds each of them using provided Seeders.
    /// </summary>
    public static async Task AddAndMigrateTenantDatabases<TDbContext, TSeeder>(
        this IServiceCollection services,
        CancellationToken       cancellationToken = default)
        where TDbContext : DbContext
        where TSeeder : class, IGenericSeeder<TDbContext>
    {
        try
        {
            using var scopeTenant = services.BuildServiceProvider().CreateScope();

            var tenantDbContext = scopeTenant.ServiceProvider.GetRequiredService<SmartAttendanceTenantDbContext>();

            if (!await tenantDbContext.Database.CanConnectAsync(cancellationToken))
                await tenantDbContext.Database.MigrateAsync(cancellationToken);

            if ((await tenantDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                await tenantDbContext.Database.MigrateAsync(cancellationToken);

            var seeder = scopeTenant.ServiceProvider.GetRequiredService<TSeeder>();

            var tenantsInDb = await tenantDbContext.TenantInfo.ToListAsync(cancellationToken);

            if (tenantsInDb.Count == 0)
                return;

            // 2. Loop through tenants
            foreach (var tenant in tenantsInDb)
            {
                try
                {
                    Console.WriteLine($"""
                                        calling Assembly {Assembly.GetEntryAssembly()!.GetName().Name} 
                                        {tenant.Identifier} - {tenant.Name} is migrating...
                                       """);

                    if (string.IsNullOrEmpty(tenant.GetConnectionString()))
                        continue;

                    using var scopeApp  = services.BuildServiceProvider().CreateScope();
                    var       dbContext = scopeApp.ServiceProvider.GetRequiredService<TDbContext>();

                    dbContext.Database.SetConnectionString(tenant.GetConnectionString());

                    if (!await dbContext.Database.CanConnectAsync(cancellationToken))
                        await dbContext.Database.MigrateAsync(cancellationToken);

                    if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                        await dbContext.Database.MigrateAsync(cancellationToken);

                    await seeder.SeedAsync(dbContext, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($@"Migration failed for tenant {tenant.Identifier}: {ex.Message}");
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fatal migration error: " + ex.Message);
            Environment.Exit(-1);
        }
    }
}