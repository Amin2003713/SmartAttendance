namespace SmartAttendance.Domain.Tenants;

public class UniversityTenantInfo : ITenantInfo
{
    public string? BranchName { get; set; }        // Branch name (e.g., Tehran, Isfahan, Mashhad)
    public string? LegalName { get; set; }         // Legal/official name if required
    public string? AccreditationCode { get; set; } // Gov/Ministry code

    // 🔹 Contact details
    public string? PhoneNumber { get; set; }
    public string? LandLine { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }

    // 🔹 Branch administration
    public Guid? BranchAdminId { get; set; }
    public UniversityAdmin? BranchAdmin { get; set; }
    public List<UniversityUser>? Users { get; set; } = [];

    // 🔹 Metadata
    public string? Logo { get; set; }
    public string? Website { get; set; }

    public bool IsPublic { get; set; } // public vs private university

    // 🔹 Tenant identity
    public string? Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now).ToString();
    public string? Identifier { get; set; } // Used for subdomain (e.g., tehran.uni-domain.com)

    // 🔹 University info
    public string? Name { get; set; }              // University name (e.g., University of Tehran)

    // 🔹 Connection string per branch
    public string GetConnectionString()
    {
        return
            $"Server={ApplicationConstant.Sql.DbServer};Database=University.{Identifier!.Replace("-", "_")};{ApplicationConstant.Sql.MultipleActiveResultSets};{ApplicationConstant.Sql.Encrypt};{ApplicationConstant.Sql.UserNameAndPass}";
    }

    // 🔹 Check if branch registration is complete
    public bool IsBranchRegistrationCompleted()
    {
        return !(string.IsNullOrEmpty(Name)        ||
                 string.IsNullOrEmpty(BranchName)  ||
                 string.IsNullOrEmpty(City)        ||
                 string.IsNullOrEmpty(Province)    ||
                 string.IsNullOrEmpty(PhoneNumber) ||
                 string.IsNullOrEmpty(Email));
    }

    // 🔹 Generate branch-specific subdomain URL
    public Uri? GetUrl()
    {
        return new Uri(
            $"{(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "http" : "https")}://{Identifier}.{ApplicationConstant.Const.BaseDomain}/");
    }

    // 🔹 Update method
    public void Update(UniversityTenantInfo request)
    {
        Name              = request.Name;
        BranchName        = request.BranchName;
        LegalName         = request.LegalName;
        AccreditationCode = request.AccreditationCode;
        PhoneNumber       = request.PhoneNumber;
        LandLine          = request.LandLine;
        Email             = request.Email;
        Address           = request.Address;
        PostalCode        = request.PostalCode;
        City              = request.City;
        Province          = request.Province;
        Logo              = request.Logo;
        Website           = request.Website;
        IsPublic          = request.IsPublic;
    }
}