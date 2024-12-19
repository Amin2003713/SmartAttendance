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
        TenantContextAccessor?.TenantInfo!.GetConnectionString()!;

    public string GetName() =>
        TenantContextAccessor?.TenantInfo!.Name!;

    public string? GetId() =>
        TenantContextAccessor?.TenantInfo!.Id!;

    public string GetIdentifier() =>
        TenantContextAccessor?.TenantInfo!.Identifier!;

    public Guid GetOwnerId() =>
        (Guid)TenantContextAccessor?.TenantInfo!.UserId!;

    public Guid GetTenantCompanyId() =>
        (Guid)TenantContextAccessor?.TenantInfo!.CompanyId!;


    private string GetTenantIdFromRequest() =>
        TenantContextAccessor?.TenantInfo!.Name!;
}

