using Shifty.Application.Commons.MediaFiles.Requests;

namespace Shifty.Application.Companies.Commands.UpdateCompany;

/// <summary>
///     Main class UpdateCompanyCommand implementing IRequest<UpdateCompanyCommandResponse>.
/// </summary>
public class UpdateCompanyCommand : IRequest
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

    public UpdateCompanyCommand AddMedia(UploadMediaFileRequest requestLogo)
    {
        Logo = requestLogo;
        return this;
    }
}