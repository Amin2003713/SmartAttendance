using SmartAttendance.Common.General.Enums.Genders;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;

public class RegisterByOwnerRequestExample : IExamplesProvider<RegisterByOwnerRequest>
{
    public RegisterByOwnerRequest GetExamples()
    {
        return new RegisterByOwnerRequest
        {
            PersonalNumber = "1234567890",
            FirstName      = "Ali",
            LastName       = "Rezaei",
            FatherName     = "Mohammad",
            NationalCode   = "0011223344",
            PhoneNumber    = "+989123456789",
            BirthDate      = new DateTime(1995, 5, 20),
            Address        = "123 Example St, Tehran, Iran",
            Gender         = GenderType.Man,
            IsActive       = true,
        };
    }
}