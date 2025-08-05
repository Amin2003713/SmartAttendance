using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Storage.Request.Queries.StorageResponses;

public record StorageResponse(

    string ItemPath,
    FileType FileType,
    decimal StorageUsedByItemMb
);