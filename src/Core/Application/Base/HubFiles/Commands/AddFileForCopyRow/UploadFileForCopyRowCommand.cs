using SmartAttendance.Common.General;

namespace SmartAttendance.Application.Base.HubFiles.Commands.AddFileForCopyRow;

public record UploadFileForCopyRowCommand(
    string          FileUrl,
    DateTime        ReportDate,
    Guid            RowId,
    FileStorageType FileStorageType
) : IRequest<MediaFileStorage>;