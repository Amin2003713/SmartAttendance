
using App.Common.Utilities.MediaFiles.Requests;
using MediatR;

namespace App.Applications.Universities.Commands.UpdateUniversity;

/// <summary>
///     Main class UpdateUniversityCommand implementing IRequest<UpdateUniversityCommandResponse>.
/// </summary>
public class UpdateUniversityCommand : IRequest
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
    public UploadMediaFileRequest Logo { get; set; }

    // 🔹 Fluent method for adding logo
    public UpdateUniversityCommand AddMedia(UploadMediaFileRequest requestLogo)
    {
        Logo = requestLogo;
        return this;
    }
}