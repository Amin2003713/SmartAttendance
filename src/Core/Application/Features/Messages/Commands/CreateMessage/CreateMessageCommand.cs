using SmartAttendance.Application.Commons.MediaFiles.Requests;
using SmartAttendance.Application.Features.Messages.Request.Commands.CreateMessage;

namespace SmartAttendance.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommand : CreateMessageRequest,
    IRequest
{
    public CreateMessageCommand AddFile(UploadMediaFileRequest imageFile)
    {
        ImageFile = imageFile;
        return this;
    }
}