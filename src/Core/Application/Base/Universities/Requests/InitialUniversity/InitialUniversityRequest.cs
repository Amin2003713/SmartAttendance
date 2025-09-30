using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Base.Universities.Requests.InitialUniversity;

public class InitialUniversityRequest
{
    public required string Domain { get; set; }
    public required string Name { get; set; }
    public string? LegalName { get; set; }
    public string? AccreditationCode { get; set; }
    public bool IsPublic { get; set; }
    public string? BranchName { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? LandLine { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? Website { get; set; }
    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    public UploadMediaFileRequest Logo { get; set; }
}