using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.EnumHelpers;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.AddRole;

public class UpdateEmployeeExample : IExamplesProvider<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequest GetExamples()
    {
        return new UpdateEmployeeRequest
        {
            Roles = new List<string>
            {
                Roles.Admin.GetEnglishName(),
                Roles.ManageProjects.GetEnglishName(),
                Roles.Projects_Create.GetEnglishName(),
                Roles.Users_Create.GetEnglishName()
            }
        };
    }
}