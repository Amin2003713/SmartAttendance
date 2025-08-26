using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.Storage.Request.Queries.StorageResponses;

public record StorageResponse(

    string ItemPath,
    FileType FileType,
    decimal StorageUsedByItemMb
);