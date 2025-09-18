using SmartAttendance.Application.Features.Users.Requests.Commands.UpdateUser;
using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : UpdateUserRequest,
    IRequest
{
    public UpdateUserCommand AddFiles(UploadMediaFileRequest? profile)
    {
        ProfilePicture = profile;
        return this;
    }
}