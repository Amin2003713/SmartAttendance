namespace Shifty.Domain.Tenants;

public class ShiftyTenantInfo : ITenantInfo
{
    public string? LandLine { get; set; }

    public string? Address { get; set; }
    public Guid? UserId { get; set; }
    public TenantAdmin? User { get; set; }
    public List<Payments.Payments>? Payments { get; set; } = [];
    public List<TenantDiscount>? TenantDiscounts { get; set; } = [];

    public string? LegalName { get; set; }

    public string? NationalCode { get; set; }

    public string? City { get; set; }

    public string? Province { get; set; }

    public string? Town { get; set; }

    public string? PostalCode { get; set; }

    public string? PhoneNumber { get; set; }
    public bool IsLegal { get; set; }
    public string? Logo { get; set; }
    public string? ActivityType { get; set; }
    public List<TenantUser>? TenantUsers { get; set; } = [];

    public string Id { get; set; } = Guid.CreateVersion7().ToString();

    public string? Identifier { get; set; }
    public string? Name { get; set; }


    public string GetConnectionString()


    {
        return
            $"Server={ApplicationConstant.Sql.DbServer};Database=Shifty.{Identifier!.Replace("-", "_")};{ApplicationConstant.Sql.MultipleActiveResultSets};{ApplicationConstant.Sql.Encrypt};{ApplicationConstant.Sql.UserNameAndPass}";
    }


    public bool IsCompanyRegistrationCompleted()
    {
        return !(string.IsNullOrEmpty(LegalName) ||
                 string.IsNullOrEmpty(NationalCode) ||
                 string.IsNullOrEmpty(City) ||
                 string.IsNullOrEmpty(Province) ||
                 string.IsNullOrEmpty(Town) ||
                 string.IsNullOrEmpty(PhoneNumber));
    }

    public Uri? GetUrl()
    {
        return new Uri(
            $"{(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "http" : "https")}://{Identifier}.{ApplicationConstant.Const.BaseDomain}/api/payment/verify");
    }

    public void Update(ShiftyTenantInfo request)
    {
        Address = request.Address;
        Name = request.Name;
        LegalName = request.LegalName;
        NationalCode = request.NationalCode;
        City = request.City;
        Province = request.Province;
        Town = request.Town;
        PostalCode = request.PostalCode;
        PhoneNumber = request.PhoneNumber;
        IsLegal = request.IsLegal;
        ActivityType = request.ActivityType;
    }
}