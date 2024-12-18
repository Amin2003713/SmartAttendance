#nullable enable
using Finbuckle.MultiTenant.Abstractions;
using Shifty.Domain.Tenants;
using System;

namespace Shifty.Persistence.TenantServices;

public interface ITenantServiceExtension
{
    IMultiTenantContext<ShiftyTenantInfo>? TenantContextAccessor { get; set; }
    string GetConnectionString();
    string GetName();
    string GetId();
    string GetIdentifier();
    Guid GetOwnerId();

    Guid GetTenantCompanyId();
}