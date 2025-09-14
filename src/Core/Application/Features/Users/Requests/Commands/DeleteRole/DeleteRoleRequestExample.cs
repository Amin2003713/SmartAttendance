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
                                   Roles.Users_Delete.GetEnglishName(),
                                   Roles.Messages_Edit.GetEnglishName()
                               })
        };
    }
}