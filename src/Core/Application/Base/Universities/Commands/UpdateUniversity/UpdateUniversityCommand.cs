using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Base.Universities.Commands.UpdateUniversity;

/// <summary>
/// Command for updating a University, fully aligned with UniversityTenantInfo model.
/// </summary>
public class UpdateUniversityCommand : IRequest
{
    // 🔹 Core University Info
    public string? Name { get; set; }
    public string? LegalName { get; set; }
    public string? AccreditationCode { get; set; } // Added for university accreditation
    public bool IsPublic { get; set; }             // Public vs private

    // 🔹 Branch Info
    public string? BranchName { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }

    // 🔹 Contact Info
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LandLine { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }

    // 🔹 Media
    public UploadMediaFileRequest Logo { get; set; }

    // 🔹 Administration
    public Guid? BranchAdminId { get; set; } // Optional update for branch admin

    // 🔹 Optional University info
    public string? ActivityType { get; set; }

    // 🔹 Fluent method for adding logo
    public UpdateUniversityCommand AddMedia(UploadMediaFileRequest requestLogo)
    {
        Logo = requestLogo;
        return this;
    }
}