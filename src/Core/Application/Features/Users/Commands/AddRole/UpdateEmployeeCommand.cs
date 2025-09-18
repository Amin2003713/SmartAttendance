using SmartAttendance.Application.Features.Users.Requests.Commands.AddRole;
using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Features.Users.Commands.AddRole;

public class UpdateEmployeeCommand : UpdateEmployeeRequest,
    IRequest
{
    public UpdateEmployeeCommand AddFiles(UploadMediaFileRequest? profile)
    {
        ProfilePicture = profile;
        return this;
    }
}