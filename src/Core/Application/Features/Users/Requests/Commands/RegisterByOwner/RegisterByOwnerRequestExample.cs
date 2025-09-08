using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.EnumHelpers;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;

public class RegisterByOwnerRequestExample : IExamplesProvider<RegisterByOwnerRequest>
{
    public RegisterByOwnerRequest GetExamples()
    {
        return new RegisterByOwnerRequest
        {
            FirstName = "نیما",
            LastName = "بیات",
            PhoneNumber = "09123456789",
            Roles =
            [
                Roles.Users_Create.GetEnglishName(), Roles.Projects_Edit.GetEnglishName(),
                Roles.Users_Edit.GetEnglishName(),
                Roles.Messages_Read.GetEnglishName()
            ]
        };
    }
}