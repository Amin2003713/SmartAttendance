using Shifty.Application.Commons.MediaFiles.Requests;
using Shifty.Application.Features.Messages.Request.Commands.CreateMessage;

namespace Shifty.Application.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommand : CreateMessageRequest,
    IRequest
{
    public CreateMessageCommand AddFile(UploadMediaFileRequest imageFile)
    {
        ImageFile = imageFile;
        return this;
    }
}