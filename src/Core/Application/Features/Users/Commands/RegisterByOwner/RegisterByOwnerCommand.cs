using SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;
using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Features.Users.Commands.RegisterByOwner;

public class RegisterByOwnerCommand : RegisterByOwnerRequest,
    IRequest
{
    public RegisterByOwnerCommand AddFiles(UploadMediaFileRequest? fileRequest)
    {
        ProfilePicture = fileRequest;
        return this;
    }
}