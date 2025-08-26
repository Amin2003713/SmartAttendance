using Shifty.Application.Base.HubFiles.Request.Commands.UploadHubFile;
using Shifty.Common.General;

namespace Shifty.Application.Base.HubFiles.Commands.UploadHubFile;

public class UploadHubFileCommand : UploadHubFileRequest,
    IRequest<MediaFileStorage>;