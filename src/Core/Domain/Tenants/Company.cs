using System;

namespace Shifty.Domain.Tenants
{
    public class Company 
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string WebSite { get; set; }

        public string PhoneNumber { get; set; }

        public string ApplicationAccessDomain { get; set; }
        public string TenantInfosId { get; set; }
        public ShiftyTenantInfo TenantInfos { get; set; }

        public bool IsActive { get; set; } = false;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
    }
}