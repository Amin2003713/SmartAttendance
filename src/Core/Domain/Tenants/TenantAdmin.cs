using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Domain.Tenants
{
    public class TenantAdmin 
    {
        public Guid Id { get; set; }  = Guid.CreateVersion7();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<ShiftyTenantInfo> Tenants { get; set; } = [];
        public DateTime RegisteredAt { get; set; }   = DateTime.Now;

        public void AddCompany(ShiftyTenantInfo company)
        {
            company.UserId = this.Id;
            Tenants.Add(company);
        }
    }
}