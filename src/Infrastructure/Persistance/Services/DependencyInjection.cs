using SmartAttendance.Persistence.Repositories.HangFire;
using SmartAttendance.Persistence.Services.Injections;

namespace SmartAttendance.Persistence.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddGenericPersistence<
            UniversityAdmin,
            Seeder.Seeder,
            HangFireJobRepository,
            SmartAttendanceDbContext, ReadOnlyDbContext, WriteOnlyDbContext>(
            (provider, connStr) =>
            {
                var identity = provider.GetRequiredService<IdentityService>();
                var options  = new DbContextOptionsBuilder<SmartAttendanceDbContext>().UseSqlServer(connStr).Options;

                return new SmartAttendanceDbContext(options, identity);
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
                    Server               = new MongoServerAddress(ApplicationConstant.Mongo.Host, ApplicationConstant.Mongo.Port),
                    DatabaseName         = ApplicationConstant.Mongo.DefaultDb,
                    Username             = ApplicationConstant.Mongo.UserName,
                    Password             = ApplicationConstant.Mongo.Password,
                    AuthenticationSource = "admin",
                    AllowInsecureTls     = true
                };

                var client = new MongoClient(mongoUrlBuilder.ToMongoUrl());
                return client.GetDatabase($"SmartAttendance{"_" + tenantDb}");
            },
            ApplicationConstant.AppOptions.TenantStore
        );


        return services;
    }

    private static DbContextOptions<SmartAttendanceDbContext> CreateContextOptions(string connectionString)
    {
        var contextOptions = new DbContextOptionsBuilder<SmartAttendanceDbContext>().UseSqlServer(connectionString).Options;

        return contextOptions;
    }
}