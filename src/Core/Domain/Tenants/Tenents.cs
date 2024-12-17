#nullable enable
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Constants;
using Shifty.Domain.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Domain.Tenants;

public class ShiftyTenantInfo : ITenantInfo
{
    public  string Id { get; set; } = Guid.CreateVersion7().ToString();
    public string Identifier { get; set; }
    public string Name { get; set; } 
    public Guid UserId { get; set; } 
    public TenantAdmin? User { get; set; } = null!;

    public Guid CompanyId { get; set; }
    public Company? Company { get; set; } = null!;


    public List<Payments>? Payments { get; set; } = [];

    public string GetConnectionString() 
        => $"Server={ApplicationConstant.DbServer};Database=Shifty.{Identifier};{ApplicationConstant.MultipleActiveResultSets};{ApplicationConstant.Encrypt};{ApplicationConstant.UserNameAndPass};";
}

public class TenantAdmin  : IdentityUser<Guid>, IEntity
{
  
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FatherName { get; set; }

    public string NationalCode { get; set; }


    public GenderType Gender { get; set; }



    public string MobileNumber { get; set; }


    public string ProfilePicture { get; set; }


    public string Address { get; set; }

    public List<ShiftyTenantInfo>? Tenants { get; set; }

    public void SetUserName() => UserName = NationalCode;

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid? ModifiedBy { get; set; }
    public DateTime ModifiedAt { get; set; }

    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime LastLoginDate { get; set; }
}

public class Payments : BaseEntity
{
    public string ShiftyTenantInfoId { get; set; }
    public ShiftyTenantInfo ShiftyTenantInfo { get; set; }
}

public class Company : BaseEntity
{
    public string Name { get; set; }

    public string Address { get; set; }

    public string WebSite { get; set; }

    public string PhoneNumber { get; set; }

    public string ApplicationAccessDomain { get; set; }
    public string TenantInfosId { get; set; }
    public ShiftyTenantInfo TenantInfos { get; set; }

}