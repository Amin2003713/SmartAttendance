
using App.Common.Utilities.MediaFiles.Requests;

namespace App.Handlers.Universities.Commands.UpdateUniversity;

/// <summary>
///     Main class UpdateUniversityCommand implementing IRequest<UpdateUniversityCommandResponse>.
/// </summary>
public class UpdateUniversityCommand : IRequest
{
    public string? Address { get; set; } = null!;
    public string? Name { get; set; }
    public string? LegalName { get; set; }
    public string NationalCode { get; set; }
    public string City { get; set; }
    public string? Province { get; set; }
    public string? Town { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsLegal { get; set; }
    public string? LandLine { get; set; }
    public UploadMediaFileRequest Logo { get; set; }

    public string? ActivityType { get; set; }

    public UpdateUniversityCommand AddMedia(UploadMediaFileRequest requestLogo)
    {
        Logo = requestLogo;
        return this;
    }
}