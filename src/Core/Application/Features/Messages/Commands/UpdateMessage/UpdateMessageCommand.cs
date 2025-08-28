using Shifty.Application.Commons.MediaFiles.Requests;
using Shifty.Application.Features.Messages.Request.Commands.UpdateMessage;

namespace Shifty.Application.Features.Messages.Commands.UpdateMessage;

public class UpdateMessageCommand : UpdateMessageRequest,
    IRequest
{
    public UpdateMessageCommand AddFile(UploadMediaFileRequest imageFile)
    {
        ImageFile = imageFile;
        return this;
    }
}