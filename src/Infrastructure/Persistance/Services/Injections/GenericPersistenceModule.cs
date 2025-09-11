using Finbuckle.MultiTenant;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Application.Interfaces.HangFire;
using SmartAttendance.Persistence.Services.MigrationManagers;

namespace SmartAttendance.Persistence.Services.Injections;

public static class GenericPersistenceModule
{
    public static IServiceCollection AddGenericPersistence<
        TAdminUser,
        TSeeder,
        THangfireRepo,
        TAppDbContext, TReadDb, TWriteDb>(
        this IServiceCollection services,
        Func<IServiceProvider, string, TAppDbContext> appDbFactory,
        Func<IServiceProvider, string, TReadDb> readDbFactory,
        Func<IServiceProvider, string, TWriteDb> writeDbFactory,
        Func<string, IMongoDatabase> mongoFactory,
        string sqlConnection)
        where TAdminUser : class
        where TSeeder : class, IGenericSeeder<TAppDbContext>
        where THangfireRepo : class, IHangFireJobRepository
        where TAppDbContext : DbContext
        where TWriteDb : DbContext
        where TReadDb : DbContext
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddScoped<IPasswordHasher<TAdminUser>, PasswordHasher<TAdminUser>>();
        services.AddKeyedScoped<UserManager<TAdminUser>>("Admins");

        services.AddDbContext<SmartAttendanceTenantDbContext>(options =>
            options.UseSqlServer(sqlConnection));

        services.AddScoped<IdentityService>();

        if (typeof(TSeeder) != typeof(Empty))
            services.AddScoped<TSeeder>();

        if (typeof(THangfireRepo) != typeof(Empty))
            services.AddScoped<IHangFireJobRepository, THangfireRepo>();

        services.AddScoped(provider => appDbFactory(provider, ResolveTenantIdentifier(provider)));

        services.AddScoped(provider => readDbFactory(provider, ResolveTenantIdentifier(provider)));

        services.AddScoped(provider => writeDbFactory(provider, ResolveTenantIdentifier(provider)));

        services.AddScoped<IMongoDatabase>(provider =>
        {
            var httpContextAccessor = provider.GetService<IHttpContextAccessor>();

            var tenant = httpContextAccessor?.HttpContext?.GetMultiTenantContext<SmartAttendanceTenantInfo>()?.TenantInfo;

            if (tenant?.Identifier != null)
                return mongoFactory(tenant.Identifier!);

            return mongoFactory("Sm");
        });

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        services.AddAndMigrateTenantDatabases<TAppDbContext, TSeeder>().ConfigureAwait(false);

        return services;
    }

    private static string ResolveTenantIdentifier(IServiceProvider provider)
    {
        var httpContextAccessor = provider.GetService<IHttpContextAccessor>();

        var tenant = httpContextAccessor?.HttpContext?.GetMultiTenantContext<SmartAttendanceTenantInfo>()?.TenantInfo;

        if (tenant is not null)
            return tenant.GetConnectionString();

        return ApplicationConstant.AppOptions.ReadDatabaseConnectionString;
    }
}