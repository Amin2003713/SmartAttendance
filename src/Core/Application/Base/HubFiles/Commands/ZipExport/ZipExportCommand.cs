using Shifty.Application.Base.HubFiles.Request.Commands.ZipExport;
using Shifty.Common.General;

namespace Shifty.Application.Base.HubFiles.Commands.ZipExport;

public class ZipExportCommand : ZipExportCommandRequest,
    IRequest<MediaFileStorage>;