namespace SmartAttendance.Domain.Tenants;

public class UniversityTenantInfo : ITenantInfo
{
    public string? LegalName { get; set; }         // Official/legal name if different
    public string? AccreditationCode { get; set; } // Government or ministry accreditation code
    public bool IsPublic { get; set; }             // Public vs private university

    // 🔹 Branch Info
    public string? BranchName { get; set; }                       // Branch location (e.g., Tehran, Isfahan)
    public string? City { get; set; }
    public string? Province { get; set; }

    // 🔹 Contact Info
    public string? PhoneNumber { get; set; }
    public string? LandLine { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? Website { get; set; }
    public string? Logo { get; set; }

    // 🔹 Administration
    public Guid? BranchAdminId { get; set; }
    public UniversityAdmin? BranchAdmin { get; set; }  // Admin object

    public List<UniversityUser>? Users { get; set; } = new(); // University users

    // 🔹 Core University Info
    public string? Id { get; set; } = Guid.NewGuid().ToString(); // Unique tenant identifier
    public string? Identifier { get; set; }                      // Used for subdomain (e.g., tehran.uni-domain.com)
    public string? Name { get; set; }                            // University name

    // 🔹 Database/Connection
    public string GetConnectionString()
    {
        if (string.IsNullOrEmpty(Identifier))
            throw new InvalidOperationException("Tenant identifier is missing");

        return
            $"Server={ApplicationConstant.Sql.DbServer};" +
            $"Database=University.{Identifier.Replace("-", "_")};" +
            $"{ApplicationConstant.Sql.MultipleActiveResultSets};" +
            $"{ApplicationConstant.Sql.Encrypt};" +
            $"{ApplicationConstant.Sql.UserNameAndPass}";
    }

    // 🔹 URL for branch/subdomain
    public Uri? GetUrl()
    {
        if (string.IsNullOrEmpty(Identifier)) return null;

        var scheme = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "http" : "https";
        return new Uri($"{scheme}://{Identifier}.{ApplicationConstant.Const.BaseDomain}/");
    }


    // 🔹 Update model with another instance
    public void Update(UniversityTenantInfo request)
    {
        if (request == null) return;

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
        BranchAdminId     = request.BranchAdminId;
    }
}