using SmartAttendance.Common.General;
using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.HubFiles.Commands.AddFileForCopyRow;

public record UploadFileForCopyRowCommand(
    string          FileUrl,
    DateTime        ReportDate,
    Guid            RowId,
    FileStorageType FileStorageType
) : IRequest<MediaFileStorage>;