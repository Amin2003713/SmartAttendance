namespace SmartAttendance.Application.Base.Universities.Requests.InitialUniversity;

public class InitialUniversityRequest
{
    public required string Domain { get; set; }

    public required string Name { get; set; }
    public string? LegalName { get; set; }         // Official/legal name if different
    public string? AccreditationCode { get; set; } // Government or ministry accreditation code
    public bool IsPublic { get; set; }             // Public vs private university

    // 🔹 Branch Info
    public string? BranchName { get; set; }                       // Branch location (e.g., Tehran, Isfahan)
    public string? City { get; set; }
    public string? Province { get; set; }

    // 🔹 Contact Info
    public string? LandLine { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? Website { get; set; }
    public required string PhoneNumber { get; set; }

    public required string FirstName { get; set; }
    public required string FatherName { get; set; }

    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}