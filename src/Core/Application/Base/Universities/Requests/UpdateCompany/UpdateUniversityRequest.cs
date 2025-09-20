using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Base.Universities.Requests.UpdateCompany;

/// <summary>
///     Represents the UpdateUniversityCommand request object.
///     Implements IExamplesProvider<UpdateUniversityCommandRequest>.
/// </summary>
public class UpdateUniversityRequest
{
    public string? Name { get; set; }
    public string? NationalCode { get; set; }
    public string? Town { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LegalName { get; set; }
    public string? AccreditationCode { get; set; }
    public bool IsPublic { get; set; }

    // 🔹 Branch Info
    public string? BranchName { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }

    // 🔹 Contact Info
    public string? LandLine { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? Website { get; set; }
    public bool IsLegal { get; set; }
    public Guid BranchAdminId { get; set; }
    public UploadMediaFileRequest? Logo { get; set; }
}