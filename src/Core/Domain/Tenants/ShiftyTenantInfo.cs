using Finbuckle.MultiTenant.Abstractions;
using Shifty.Domain.Constants;
using System;
using System.Collections.Generic;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Shifty.Domain.Tenants
{
    public class ShiftyTenantInfo : ITenantInfo
    {
        public string? LandLine { get; set; } = null!;

        public string? Address { get; set; } = null!;
        public Guid? UserId { get; set; } = null!;
        public TenantAdmin? User { get; set; } = null!;
        public List<Payments>? Payments { get; set; } = [];
        public string? Id { get; set; } = Guid.CreateVersion7().ToString();
        public string? Identifier { get; set; }
        public string? Name { get; set; }


        public string GetConnectionString()
        {
            return
                $"Server={ApplicationConstant.DbServer};Database=Shifty.{Identifier};{ApplicationConstant.MultipleActiveResultSets};{ApplicationConstant.Encrypt};{ApplicationConstant.UserNameAndPass}";
        }
    }

    public class Logo
    {
        public Guid Id { get; set; }
        public string? Value { get; set; }
        public DateTime CreateAt { get; set; }
        public Guid CompanyId { get; set; }
    }
}