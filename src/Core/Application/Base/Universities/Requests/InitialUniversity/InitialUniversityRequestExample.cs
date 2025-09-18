namespace SmartAttendance.Application.Base.Universities.Requests.InitialUniversity;

public class InitialUniversityRequestExample : IExamplesProvider<InitialUniversityRequest>
{
    public InitialUniversityRequest GetExamples()
    {
        return new InitialUniversityRequest
        {
            Name            = "Islamic Azad University, Esfahan (Khorasgan) Branch",
            Domain          = "iau-esfahan",
            PhoneNumber     = "03135354000",
            FirstName       = "Mohammad",
            LastName        = "Ahmadi",
            NationalCode    = "0034567891",
            FatherName      = "Hossein",
            Password        = "@EsfUni123",
            ConfirmPassword = "@EsfUni123"
        };
    }
}