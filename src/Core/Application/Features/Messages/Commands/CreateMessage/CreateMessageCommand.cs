using Shifty.Application.Commons.MediaFiles.Requests;
using Shifty.Application.Features.Messages.Request.Commands.CreateMessage;

namespace Shifty.Application.Features.Messages.Commands.CreateMessage;

public class CreateCommand : CreateMessageRequest,
    IRequest
{
    public CreateCommand AddFile(UploadMediaFileRequest imageFile)
    {
        ImageFile = imageFile;
        return this;
    }
}