using SmartAttendance.Application.Base.HubFiles.Request.Commands.UploadHubFile;
using SmartAttendance.Common.General;

namespace SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;

public class UploadHubFileCommand : UploadHubFileRequest,
    IRequest<MediaFileStorage>;