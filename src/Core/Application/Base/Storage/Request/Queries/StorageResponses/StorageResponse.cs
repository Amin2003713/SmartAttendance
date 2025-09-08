using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.Storage.Request.Queries.StorageResponses;

public record StorageResponse(
    string ItemPath,
    FileType FileType,
    decimal StorageUsedByItemMb
);