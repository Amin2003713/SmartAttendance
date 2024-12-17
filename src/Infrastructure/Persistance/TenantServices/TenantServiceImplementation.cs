#nullable enable
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shifty.Persistence.TenantServices;

public class TenantServiceImplementation : ITenantService
{
    public IMultiTenantContext<ShiftyTenantInfo>? _httpContextAccessor { get; set; }

    public string GetConnectionString()
    {
        return _httpContextAccessor?.TenantInfo!.GetConnectionString()!;
    }

    public string GetName()
    {
        return _httpContextAccessor?.TenantInfo!.Name!;
    }

    public string GetId()
    {
        return _httpContextAccessor?.TenantInfo!.Id!;
    }

    public string GetIdentifier()
    {
        return _httpContextAccessor?.TenantInfo!.Identifier!;
    }

    public Guid GetOwnerId()
    {
        return (Guid)_httpContextAccessor?.TenantInfo!.UserId!;
    }

    public Guid GetTenantCompanyId()
    {
        return (Guid)_httpContextAccessor?.TenantInfo!.CompanyId!;
    }


    private string GetTenantIdFromRequest()
    {
        return _httpContextAccessor?.TenantInfo!.Name!;
    }
}

public class TenantMigrationService(IServiceProvider serviceProvider)
{
    public async Task MigrateTenantDatabaseAsync()
    {
        // Update the connection string for the tenant
        var tenantService = serviceProvider.GetRequiredService<ITenantService>();
        

        // Use the TenantDbContext for migrations
        using var scope   = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WriteOnlyDbContext>();

        context = new WriteOnlyDbContext(CreateContextOptions(tenantService.GetConnectionString()), tenantService);
        
        if(await context.Database.EnsureCreatedAsync() && (await context.Database.GetPendingMigrationsAsync()).Any())
            await context.Database.MigrateAsync();
    }

    DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
    {
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;

        return contextOptions;
    }
}
