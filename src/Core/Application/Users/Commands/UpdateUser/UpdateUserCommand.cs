using Shifty.Application.Commons.MediaFiles.Requests;
using Shifty.Application.Users.Requests.Commands.UpdateUser;

namespace Shifty.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : UpdateUserRequest,
    IRequest
{
    public UpdateUserCommand AddFiles(UploadMediaFileRequest profile)
    {
        ImageFile = profile;
        return this;
    }
}