namespace SmartAttendance.Application.Base.Companies.Requests.UpdateCompany;

/// <summary>
///     A separate example provider for UpdateCompanyRequest if desired.
///     You might remove this if UpdateCompanyRequest itself implements IExamplesProvider.
/// </summary>
public class UpdateCompanyRequestExample : IExamplesProvider<UpdateCompanyRequest>
{
    public UpdateCompanyRequest GetExamples()
    {
        return new UpdateCompanyRequest
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