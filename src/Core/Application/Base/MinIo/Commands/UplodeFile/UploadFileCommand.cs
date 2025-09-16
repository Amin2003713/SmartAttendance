using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Base.MinIo.Requests.Commands.UploadFile;

namespace SmartAttendance.Application.Base.MinIo.Commands.UplodeFile;

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