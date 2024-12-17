using Finbuckle.MultiTenant.Abstractions;
using Shifty.Domain.Tenants;
using System;

namespace Shifty.Persistence.TenantServices;

public interface ITenantService
{
    IMultiTenantContext<ShiftyTenantInfo> _httpContextAccessor { get; set; }
    string GetConnectionString();
    string GetName();
    string GetId();
    string GetIdentifier();
    Guid GetOwnerId();

    Guid GetTenantCompanyId();
}