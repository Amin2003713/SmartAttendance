using Shifty.Application.HubFiles.Request.Commands.ZipExport;
using Shifty.Common.General;

namespace Shifty.Application.HubFiles.Commands.ZipExport;

public class ZipExportCommand : ZipExportCommandRequest,
    IRequest<MediaFileStorage>;