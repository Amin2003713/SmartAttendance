using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Domain.Tenants
{
    public class TenantAdmin : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FatherName { get; set; }

        public string NationalCode { get; set; }


        public GenderType Gender { get; set; }


        public string ProfilePicture { get; set; }


        public string Address { get; set; }

        public List<ShiftyTenantInfo>? Tenants { get; set; }

        public void SetUserName()
        {
            UserName = NationalCode;
        }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime LastLoginDate { get; set; }

        public void SetIdentityFields()
        {
            NormalizedEmail = Email?.ToUpper();
            NormalizedUserName = UserName?.ToUpper();
            SecurityStamp = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }
    }
}