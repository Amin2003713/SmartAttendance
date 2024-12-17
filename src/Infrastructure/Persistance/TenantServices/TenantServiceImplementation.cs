using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
using Shifty.Domain.Tenants;
using System;

namespace Shifty.Persistence.TenantServices;

public class TenantServiceImplementation : ITenantService
{
    public IMultiTenantContext<ShiftyTenantInfo> _httpContextAccessor { get; set; }

    public string GetConnectionString()
    {
        return _httpContextAccessor.TenantInfo!.GetConnectionString();
    }

    public string GetName()
    {
        return _httpContextAccessor.TenantInfo!.Name;
    }

    public string GetId()
    {
        return _httpContextAccessor.TenantInfo!.Id;
    }

    public string GetIdentifier()
    {
        return _httpContextAccessor.TenantInfo!.Identifier;
    }

    public Guid GetOwnerId()
    {
        return _httpContextAccessor.TenantInfo!.UserId;
    }

    public Guid GetTenantCompanyId()
    {
        return _httpContextAccessor.TenantInfo!.CompanyId;
    }


    private string GetTenantIdFromRequest()
    {
        return _httpContextAccessor.TenantInfo!.Name;
    }
}