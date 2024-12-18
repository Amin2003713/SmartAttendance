using System;

namespace Shifty.Domain.Tenants
{
    public class Payments 
    {
        public string ShiftyTenantInfoId { get; set; }
        public ShiftyTenantInfo ShiftyTenantInfo { get; set; }
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