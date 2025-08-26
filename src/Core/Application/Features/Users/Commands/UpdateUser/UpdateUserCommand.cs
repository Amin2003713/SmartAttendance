using Shifty.Application.Commons.MediaFiles.Requests;
using Shifty.Application.Features.Users.Requests.Commands.UpdateUser;

namespace Shifty.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : UpdateUserRequest,
    IRequest
{
    public UpdateUserCommand AddFiles(UploadMediaFileRequest profile)
    {
        ImageFile = profile;
        return this;
    }
}