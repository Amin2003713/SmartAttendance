using Mapster;
using Shifty.Application.HubFiles.Commands.UploadHubFile;
using Shifty.Application.MinIo.Requests.Commands.UploadFile;
using Shifty.Domain.HubFiles;

namespace Shifty.Application.MinIo.Commands.UplodeFile;

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