#nullable enable
using Finbuckle.MultiTenant.Abstractions;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Constants;
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
        => $"Server={ApplicationConstant.DbServer};Database=Shifty.{Identifier};{ApplicationConstant.MultipleActiveResultSets};{ApplicationConstant.Encrypt};{ApplicationConstant.UserNameAndPass}";
}