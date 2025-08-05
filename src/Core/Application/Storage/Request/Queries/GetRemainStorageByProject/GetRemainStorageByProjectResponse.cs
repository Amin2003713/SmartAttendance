namespace Shifty.Application.Storage.Request.Queries.GetRemainStorageByProject;

public record GetRemainStorageByProjectResponse(
    decimal TotalStorageMb,
    decimal UsedStorageMb,
    decimal AvailableStorageMb,
    bool StorageWarning
);