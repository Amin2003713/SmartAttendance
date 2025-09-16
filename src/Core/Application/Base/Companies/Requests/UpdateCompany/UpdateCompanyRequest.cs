namespace SmartAttendance.Application.Base.Companies.Requests.UpdateCompany;

/// <summary>
///     Represents the UpdateCompanyCommand request object.
///     Implements IExamplesProvider<UpdateCompanyCommandRequest>.
/// </summary>
public class UpdateCompanyRequest
{
    public string? Address { get; set; } = null!;
    public string? Name { get; set; }
    public string? LegalName { get; set; }
    public string? NationalCode { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? Town { get; set; }
    public string? PostalCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LandLine { get; set; }

    public bool IsLegal { get; set; }
    public UploadMediaFileRequest? Logo { get; set; }
    public string? ActivityType { get; set; }
}