using Shifty.Application.HubFiles.Request.Commands.UploadHubFile;
using Shifty.Common.General;

namespace Shifty.Application.HubFiles.Commands.UploadHubFile;

public class UploadHubFileCommand : UploadHubFileRequest,
    IRequest<MediaFileStorage>;