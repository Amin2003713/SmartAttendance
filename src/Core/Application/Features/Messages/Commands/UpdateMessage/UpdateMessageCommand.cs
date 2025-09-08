using SmartAttendance.Application.Commons.MediaFiles.Requests;
using SmartAttendance.Application.Features.Messages.Request.Commands.UpdateMessage;

namespace SmartAttendance.Application.Features.Messages.Commands.UpdateMessage;

public class UpdateMessageCommand : UpdateMessageRequest,
    IRequest
{
    public UpdateMessageCommand AddFile(UploadMediaFileRequest imageFile)
    {
        ImageFile = imageFile;
        return this;
    }
}