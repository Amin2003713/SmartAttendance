using Mapster;
using Shifty.Application.Base.HubFiles.Commands.UploadHubFile;
using Shifty.Application.Base.MinIo.Requests.Commands.UploadFile;
using Shifty.Domain.HubFiles;

namespace Shifty.Application.Base.MinIo.Commands.UplodeFile;

public class UploadFileCommand : UploadFileRequest,
    IRequest<HubFile>
{
    public string Path { get; set; }

    public static UploadFileCommand Create(UploadHubFileCommand request, string path)
    {
        var result = request.Adapt<UploadFileCommand>();
        result.Path = path;
        result.File = request.File;
        return result;
    }
}