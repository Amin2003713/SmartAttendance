using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.Utilities.EnumHelpers;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.DeleteRole;

public class DeleteRoleRequestExample : IExamplesProvider<DeleteRoleRequest>
{
    public DeleteRoleRequest GetExamples()
    {
        return new DeleteRoleRequest
        {
            Role = string.Join(",",
                new List<string>
                {
                    RolesType.Users_Delete.GetEnglishName(),
                    RolesType.Messages_Edit.GetEnglishName()
                })
        };
    }
}