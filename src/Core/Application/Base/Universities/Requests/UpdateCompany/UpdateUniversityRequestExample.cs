namespace SmartAttendance.Application.Base.Universities.Requests.UpdateCompany;

/// <summary>
///     A separate example provider for UpdateUniversityRequest if desired.
///     You might remove this if UpdateUniversityRequest itself implements IExamplesProvider.
/// </summary>
public class UpdateUniversityRequestExample : IExamplesProvider<UpdateUniversityRequest>
{
    public UpdateUniversityRequest GetExamples()
    {
        return new UpdateUniversityRequest
        {
            Address      = "No. 10, Some Street, Some City",
            Name         = "Tech Solutions Ltd.",
            LegalName    = "Tech Solutions Legal",
            NationalCode = "1234567890",
            City         = "Tehran",
            Province     = "Tehran",
            Town         = "Some Town",
            PostalCode   = "1234567890",
            PhoneNumber  = "+989121234567",
            IsLegal      = true,
            ActivityType = "Software Development"
        };
    }
}