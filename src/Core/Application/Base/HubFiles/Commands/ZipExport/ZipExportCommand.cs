using SmartAttendance.Application.Base.HubFiles.Request.Commands.ZipExport;
using SmartAttendance.Common.General;

namespace SmartAttendance.Application.Base.HubFiles.Commands.ZipExport;

public class ZipExportCommand : ZipExportCommandRequest,
    IRequest<MediaFileStorage>;