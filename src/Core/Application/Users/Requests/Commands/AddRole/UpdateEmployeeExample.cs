using Shifty.Common.General.Enums;
using Shifty.Common.Utilities.EnumHelpers;

namespace Shifty.Application.Users.Requests.Commands.AddRole;

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