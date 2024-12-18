using System;

namespace Shifty.Application.Tenants.Command
{
    public class CreateTenantResponse
    {

        public Guid TenantId { get; set; }
        public string TenantDomain { get; set; }

    }
}