namespace Shifty.Application.Base.Storage.Request.Queries.GetRemainStorage;

public record GetRemainStorageResponse(
    decimal TotalStorageMb,
    decimal UsedStorageMb,
    decimal AvailableStorageMb,
    bool StorageWarning
);