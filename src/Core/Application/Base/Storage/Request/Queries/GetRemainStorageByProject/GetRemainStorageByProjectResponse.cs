namespace Shifty.Application.Base.Storage.Request.Queries.GetRemainStorageByProject;

public record GetRemainStorageByProjectResponse(
    decimal TotalStorageMb,
    decimal UsedStorageMb,
    decimal AvailableStorageMb,
    bool StorageWarning
);