using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.EnumHelpers;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;

public class RegisterByOwnerRequestExample : IExamplesProvider<RegisterByOwnerRequest>
{
    public RegisterByOwnerRequest GetExamples()
    {
        return new RegisterByOwnerRequest
        {
            FirstName   = "نیما",
            LastName    = "بیات",
            PhoneNumber = "09123456789",
            Roles =
            [
                RolesType.Users_Create.GetEnglishName(), RolesType.Projects_Edit.GetEnglishName(),
                RolesType.Users_Edit.GetEnglishName(),
                RolesType.Messages_Read.GetEnglishName()
            ]
        };
    }
}