using SmartAttendance.Application.Features.Users.Requests.Commands.UpdateUser;

namespace SmartAttendance.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : UpdateUserRequest,
                                 IRequest
{
    public UpdateUserCommand AddFiles(UploadMediaFileRequest profile)
    {
        ImageFile = profile;
        return this;
    }
}