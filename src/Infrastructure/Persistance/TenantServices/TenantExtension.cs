#nullable enable
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shifty.Common;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shifty.Persistence.TenantServices;

public class TenantExtension : IScopedDependency , ITenantServiceExtension 
{
    public IMultiTenantContext<ShiftyTenantInfo>? TenantContextAccessor { get; set; }

    public string GetConnectionString() =>
        TenantContextAccessor?.TenantInfo?.GetConnectionString()! ?? null!;

    public string GetName() =>
        TenantContextAccessor?.TenantInfo?.Name! ?? null!;

    public string? GetId() =>
        TenantContextAccessor?.TenantInfo?.Id ?? null;

    public string GetIdentifier() =>
        TenantContextAccessor?.TenantInfo?.Identifier! ?? null!;

    public Guid GetOwnerId() =>
        (Guid)TenantContextAccessor?.TenantInfo?.UserId!;

    public string GetDomain() =>
        TenantContextAccessor?.TenantInfo?.Identifier! ?? null!;
    
    public ShiftyTenantInfo GetTenantInfo() =>
        TenantContextAccessor?.TenantInfo ?? null!;


    private string GetTenantIdFromRequest() =>
        TenantContextAccessor?.TenantInfo?.Name! ?? null!;
}

