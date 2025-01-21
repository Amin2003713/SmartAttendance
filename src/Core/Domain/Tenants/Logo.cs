using System;

namespace Shifty.Domain.Tenants
{
    public class Logo
    {
        public Guid Id { get; set; }
        public string? Value { get; set; }
        public DateTime CreateAt { get; set; }
        public Guid CompanyId { get; set; }
    }
}