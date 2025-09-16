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
                RolesType.Admin.GetEnglishName(),
                RolesType.ManageProjects.GetEnglishName(),
                RolesType.Projects_Create.GetEnglishName(),
                RolesType.Users_Create.GetEnglishName()
            }
        };
    }
}