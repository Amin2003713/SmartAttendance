#nullable enable
using Finbuckle.MultiTenant.Abstractions;
using Shifty.Common;
using Shifty.Domain.Tenants;
using System;

namespace Shifty.Persistence.TenantServices
{
    public class TenantExtension : IScopedDependency , ITenantServiceExtension
    {
        public IMultiTenantContext<ShiftyTenantInfo>? TenantContextAccessor { get; set; }

        public string GetConnectionString()
        {
            return TenantContextAccessor?.TenantInfo?.GetConnectionString()! ?? null!;
        }

        public string GetName()
        {
            return TenantContextAccessor?.TenantInfo?.Name! ?? null!;
        }

        public string? GetId()
        {
            return TenantContextAccessor?.TenantInfo?.Id ?? null;
        }

        public string GetIdentifier()
        {
            return TenantContextAccessor?.TenantInfo?.Identifier! ?? null!;
        }

        public Guid GetOwnerId()
        {
            return (Guid)TenantContextAccessor?.TenantInfo?.UserId!;
        }

        public string GetDomain()
        {
            return TenantContextAccessor?.TenantInfo?.Identifier! ?? null!;
        }

        public ShiftyTenantInfo GetTenantInfo()
        {
            return TenantContextAccessor?.TenantInfo ?? null!;
        }


        private string GetTenantIdFromRequest()
        {
            return TenantContextAccessor?.TenantInfo?.Name! ?? null!;
        }
    }
}