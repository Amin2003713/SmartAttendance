using Shifty.Common.General;
using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.HubFiles.Commands.AddFileForCopyRow;

public record UploadFileForCopyRowCommand(
    string FileUrl,
    
    DateTime ReportDate,
    Guid RowId,
    FileStorageType FileStorageType
) : IRequest<MediaFileStorage>;