using Shifty.Persistence.Repositories.HangFire;
using Shifty.Persistence.Services.Injections;

namespace Shifty.Persistence.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddGenericPersistence<
            TenantAdmin,
            Seeder.Seeder,
            HangFireJobRepository,
            ShiftyDbContext, ReadOnlyDbContext, WriteOnlyDbContext>(
            (provider, connStr) =>
            {
                var identity = provider.GetRequiredService<IdentityService>();
                var options = new DbContextOptionsBuilder<ShiftyDbContext>()
                    .UseSqlServer(connStr)
                    .Options;

                return new ShiftyDbContext(options, identity);
            },
            (provider, connStr) =>
            {
                var options = CreateContextOptions(connStr);

                var identity = provider.GetRequiredService<IdentityService>();
                return new ReadOnlyDbContext(options, identity);
            },
            (provider, connStr) =>
            {
                var identity = provider.GetRequiredService<IdentityService>();
                var options  = CreateContextOptions(connStr);
                return new WriteOnlyDbContext(options, identity);
            },
            tenantDb =>
            {
                var mongoUrlBuilder = new MongoUrlBuilder
                {
                    Server = new MongoServerAddress(ApplicationConstant.Mongo.Host, ApplicationConstant.Mongo.Port),
                    DatabaseName = ApplicationConstant.Mongo.DefaultDb,
                    Username = ApplicationConstant.Mongo.UserName,
                    Password = ApplicationConstant.Mongo.Password,
                    AuthenticationSource = "admin",
                    AllowInsecureTls = true
                };

                var client = new MongoClient(mongoUrlBuilder.ToMongoUrl());
                return client.GetDatabase($"Shifty{"_" + tenantDb}");
            },
            ApplicationConstant.AppOptions.TenantStore
        );


        return services;
    }

    private static DbContextOptions<ShiftyDbContext> CreateContextOptions(string connectionString)
    {
        var contextOptions = new DbContextOptionsBuilder<ShiftyDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return contextOptions;
    }
}