namespace SmartAttendance.Application.Base.Universities.Requests.InitialUniversity;

public class InitialUniversityRequestExample : IExamplesProvider<InitialUniversityRequest>
{
    public InitialUniversityRequest GetExamples()
    {
        return new InitialUniversityRequest
        {
            Name            = "Islamic Azad University, Esfahan (Khorasgan) Branch",
            Domain          = "iau-esfahan",
            LegalName        = "University of Tehran (Official)",
            AccreditationCode = "IR-UT-001",
            IsPublic         = true,

            BranchName       = "Main Branch",
            City             = "Tehran",
            Province         = "Tehran Province",

            LandLine         = "021-12345678",
            Email            = "info@ut.ac.ir",
            Address          = "No. 1, University Blvd, Tehran",
            PostalCode       = "1234567890",
            Website          = "https://www.ut.ac.ir",
            PhoneNumber      = "09121234567",

            FirstName        = "Ali",
            FatherName       = "Reza",
            LastName         = "Ahmadi",

            Password        = "@EsfUni123",
            ConfirmPassword = "@EsfUni123"
        };
    }
}